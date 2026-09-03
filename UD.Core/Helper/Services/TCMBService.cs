namespace UD.Core.Helper.Services
{
    using System;
    using System.Collections.Concurrent;
    using System.Globalization;
    using System.Text;
    using System.Xml.Linq;
    using UD.Core.Enums;
    using UD.Core.Extensions;
    using UD.Core.Helper.Responses;
    public interface ITCMBService // AddSingleton
    {
        Task<TCMBResponse> GetAsync(EnumTCMBRateCode rateCode, DateOnly? date = null, CancellationToken cancellationToken = default);
        Task<TCMBResponse> GetUSDAsync(DateOnly? date = null, CancellationToken cancellationToken = default);
        Task<TCMBResponse> GetEURAsync(DateOnly? date = null, CancellationToken cancellationToken = default);
    }
    public sealed class TCMBService : ITCMBService
    {
        private sealed record XmlCacheItem(int index, XDocument xml);
        private readonly ConcurrentDictionary<DateTime, XmlCacheItem> _dicXmlCache = new();
        private int _cacheIndex;
        public TCMBService() { }
        private async Task<XDocument> GetXmlAsync(DateTime date, CancellationToken cancellationToken)
        {
            if (this._dicXmlCache.TryGetValue(date, out var _cachedXml)) { return _cachedXml.xml; }
            var (hasError, dataBinary, _, ex) = await GetUrl(date).GetBinaryDataAsync(TimeSpan.FromSeconds(5), cancellationToken);
            if (hasError) { throw ex; }
            var parsedXml = XDocument.Parse(Encoding.UTF8.GetString(dataBinary));
            var doc = this._dicXmlCache.GetOrAdd(date, _ => new(Interlocked.Increment(ref this._cacheIndex), parsedXml));
            if (this._dicXmlCache.Count > 15)
            {
                var oldestIndexItemKey = this._dicXmlCache.OrderBy(k => k.Value.index).Select(x => x.Key).FirstOrDefault();
                this._dicXmlCache.TryRemove(oldestIndexItemKey, out _);
            }
            return doc.xml;
        }
        private static Uri GetUrl(DateTime date) => new(date == DateTime.Today ? "https://www.tcmb.gov.tr/kurlar/today.xml" : $"https://www.tcmb.gov.tr/kurlar/{date:yyyyMM}/{date:ddMMyyyy}.xml");
        private static decimal ParseDecimalValue(XElement element) => (Decimal.TryParse(element?.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out var _result) ? _result : default);
        private static TCMBResponse GetRate(XDocument xml, string code)
        {
            var node = xml.Descendants("Currency").FirstOrDefault(x => x.Attribute("CurrencyCode")?.Value == code);
            var data = new TCMBResponse();
            if (Int32.TryParse(node.Element(nameof(data.Unit))?.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out var _valueInt)) { data.Unit = _valueInt; }
            data.ForexBuying = ParseDecimalValue(node.Element(nameof(data.ForexBuying)));
            data.ForexSelling = ParseDecimalValue(node.Element(nameof(data.ForexSelling)));
            data.BanknoteBuying = ParseDecimalValue(node.Element(nameof(data.BanknoteBuying)));
            data.BanknoteSelling = ParseDecimalValue(node.Element(nameof(data.BanknoteSelling)));
            return data;
        }
        public async Task<TCMBResponse> GetAsync(EnumTCMBRateCode rateCode, DateOnly? date = null, CancellationToken cancellationToken = default)
        {
            var dateTime = (date.HasValue ? date.Value.ToDateTime(default) : DateTime.Today);
            if (dateTime.DayOfWeek.IsWeekDays()) { return GetRate(await this.GetXmlAsync(dateTime, cancellationToken), rateCode.ToString("g")); }
            return new();
        }
        public Task<TCMBResponse> GetUSDAsync(DateOnly? date = null, CancellationToken cancellationToken = default) => this.GetAsync(EnumTCMBRateCode.USD, date, cancellationToken);
        public Task<TCMBResponse> GetEURAsync(DateOnly? date = null, CancellationToken cancellationToken = default) => this.GetAsync(EnumTCMBRateCode.EUR, date, cancellationToken);
    }
}