namespace UD.Core.Helper.TCMBKur
{
    using System;
    using System.Collections.Concurrent;
    using System.Globalization;
    using System.Text;
    using System.Xml.Linq;
    using UD.Core.Extensions;
    using static UD.Core.Enums.CDayOfWeekTR;
    public interface ITCMBKurService // AddSingleton
    {
        Task<TCMBKurResponse> Get(TCMBKurCodeTypes type, DateOnly? date = null, CancellationToken cancellationToken = default);
        Task<TCMBKurResponse> GetUSD(DateOnly? date = null, CancellationToken cancellationToken = default);
        Task<TCMBKurResponse> GetEUR(DateOnly? date = null, CancellationToken cancellationToken = default);
    }
    public sealed class TCMBKurService : ITCMBKurService
    {
        private readonly ConcurrentDictionary<DateTime, XDocument> dicXmlCache = new();
        public TCMBKurService() { }
        private async Task<XDocument> GetXml(DateTime date, CancellationToken cancellationToken)
        {
            if (this.dicXmlCache.TryGetValue(date, out XDocument _cachedXml)) { return _cachedXml; }
            var (hasError, dataBinary, _, ex) = await this.GetUrl(date).GetBinaryData(TimeSpan.FromSeconds(5), cancellationToken);
            if (hasError) { throw ex; }
            return this.dicXmlCache.GetOrAdd(date, XDocument.Parse(Encoding.UTF8.GetString(dataBinary)));
        }
        private Uri GetUrl(DateTime date) => new(date == DateTime.Today ? "https://www.tcmb.gov.tr/kurlar/today.xml" : $"https://www.tcmb.gov.tr/kurlar/{date:yyyyMM}/{date:ddMMyyyy}.xml");
        private TCMBKurResponse GetRate(XDocument xml, string code)
        {
            var node = xml.Descendants("Currency").FirstOrDefault(x => x.Attribute("CurrencyCode")?.Value == code);
            if (node == null) { throw new Exception($"Kur bilgisi alınamadı: \"{code}\""); }
            var data = new TCMBKurResponse();
            if (Int32.TryParse(node.Element(nameof(data.Unit))?.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out int _valueInt)) { data.Unit = _valueInt; }
            if (Decimal.TryParse(node.Element(nameof(data.ForexBuying))?.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal _valueDecimal)) { data.ForexBuying = _valueDecimal; }
            if (Decimal.TryParse(node.Element(nameof(data.ForexSelling))?.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out _valueDecimal)) { data.ForexSelling = _valueDecimal; }
            if (Decimal.TryParse(node.Element(nameof(data.BanknoteBuying))?.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out _valueDecimal)) { data.BanknoteBuying = _valueDecimal; }
            if (Decimal.TryParse(node.Element(nameof(data.BanknoteSelling))?.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out _valueDecimal)) { data.BanknoteSelling = _valueDecimal; }
            if (data.Equals(new())) { throw new Exception($"Kur bilgisi alınamadı: \"{code}\""); }
            return data;
        }
        public async Task<TCMBKurResponse> Get(TCMBKurCodeTypes type, DateOnly? date = null, CancellationToken cancellationToken = default)
        {
            var dateTime = (date.HasValue ? date.Value.ToDateTime(default) : DateTime.Today);
            if (IsWeekDays((DayOfWeekTR)dateTime.DayOfWeek)) { return this.GetRate(await this.GetXml(dateTime, cancellationToken), type.ToString("g")); }
            return new();
        }
        public Task<TCMBKurResponse> GetUSD(DateOnly? date = null, CancellationToken cancellationToken = default) => this.Get(TCMBKurCodeTypes.USD, date, cancellationToken);
        public Task<TCMBKurResponse> GetEUR(DateOnly? date = null, CancellationToken cancellationToken = default) => this.Get(TCMBKurCodeTypes.EUR, date, cancellationToken);
    }
}