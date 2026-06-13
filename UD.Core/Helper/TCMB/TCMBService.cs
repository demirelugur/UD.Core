namespace UD.Core.Helper.TCMB
{
    using System;
    using System.Collections.Concurrent;
    using System.Globalization;
    using System.Text;
    using System.Xml.Linq;
    using UD.Core.Extensions;
    using UD.Core.Helper.Validation;
    public interface ITCMBService // AddSingleton
    {
        Task<TCMBResponse> Get(EnumTCMBRateCode rateCode, DateOnly? date = null, CancellationToken cancellationToken = default);
        Task<TCMBResponse> GetUSD(DateOnly? date = null, CancellationToken cancellationToken = default);
        Task<TCMBResponse> GetEUR(DateOnly? date = null, CancellationToken cancellationToken = default);
    }
    public sealed class TCMBService : ITCMBService
    {
        private sealed record XmlCacheItem(ulong index, XDocument xml);
        private readonly ConcurrentDictionary<DateTime, XmlCacheItem> dicXmlCache = new();
        private ulong cacheIndex;
        public TCMBService() { }
        private async Task<XDocument> GetXml(DateTime date, CancellationToken cancellationToken)
        {
            if (this.dicXmlCache.TryGetValue(date, out XmlCacheItem _cachedXml)) { return _cachedXml.xml; }
            var (hasError, dataBinary, _, ex) = await this.GetUrl(date).GetBinaryData(TimeSpan.FromSeconds(5), cancellationToken);
            if (hasError) { throw ex; }
            var parsedXml = XDocument.Parse(Encoding.UTF8.GetString(dataBinary));
            var doc = this.dicXmlCache.GetOrAdd(date, _ => new(Interlocked.Increment(ref this.cacheIndex), parsedXml));
            if (this.dicXmlCache.Count > 15)
            {
                var oldestIndexItemKey = this.dicXmlCache.OrderBy(k => k.Value.index).Select(x => x.Key).FirstOrDefault();
                this.dicXmlCache.TryRemove(oldestIndexItemKey, out _);
            }
            return doc.xml;
        }
        private Uri GetUrl(DateTime date) => new(date == DateTime.Today ? "https://www.tcmb.gov.tr/kurlar/today.xml" : $"https://www.tcmb.gov.tr/kurlar/{date:yyyyMM}/{date:ddMMyyyy}.xml");
        private TCMBResponse GetRate(XDocument xml, string code)
        {
            var node = xml.Descendants("Currency").FirstOrDefault(x => x.Attribute("CurrencyCode")?.Value == code);
            Guard.ThrowIfNull(node, nameof(node));
            var data = new TCMBResponse();
            if (Int32.TryParse(node.Element(nameof(data.Unit))?.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out int _valueInt)) { data.Unit = _valueInt; }
            if (Decimal.TryParse(node.Element(nameof(data.ForexBuying))?.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal _valueDecimal)) { data.ForexBuying = _valueDecimal; }
            if (Decimal.TryParse(node.Element(nameof(data.ForexSelling))?.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out _valueDecimal)) { data.ForexSelling = _valueDecimal; }
            if (Decimal.TryParse(node.Element(nameof(data.BanknoteBuying))?.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out _valueDecimal)) { data.BanknoteBuying = _valueDecimal; }
            if (Decimal.TryParse(node.Element(nameof(data.BanknoteSelling))?.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out _valueDecimal)) { data.BanknoteSelling = _valueDecimal; }
            return data;
        }
        public async Task<TCMBResponse> Get(EnumTCMBRateCode rateCode, DateOnly? date = null, CancellationToken cancellationToken = default)
        {
            var dateTime = (date.HasValue ? date.Value.ToDateTime(default) : DateTime.Today);
            if (dateTime.DayOfWeek.IsWeekDays()) { return this.GetRate(await this.GetXml(dateTime, cancellationToken), rateCode.ToString("g")); }
            return new();
        }
        public Task<TCMBResponse> GetUSD(DateOnly? date = null, CancellationToken cancellationToken = default) => this.Get(EnumTCMBRateCode.USD, date, cancellationToken);
        public Task<TCMBResponse> GetEUR(DateOnly? date = null, CancellationToken cancellationToken = default) => this.Get(EnumTCMBRateCode.EUR, date, cancellationToken);
    }
}