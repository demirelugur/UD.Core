namespace UD.Core.Helper.TCMBKur
{
    using System;
    using System.Globalization;
    using System.Text;
    using System.Xml.Linq;
    using UD.Core.Extensions;
    public interface ITCMBKurService
    {
        Task<TCMBKurDto> GetAsync(TCMBCurrencyCodeTypes type, DateOnly? date = null, CancellationToken cancellationToken = default);
        Task<TCMBKurDto> GetUSDAsync(DateOnly? date = null, CancellationToken cancellationToken = default);
        Task<TCMBKurDto> GetEURAsync(DateOnly? date = null, CancellationToken cancellationToken = default);
    }
    public sealed class TCMBKurService : ITCMBKurService
    {
        private DateOnly? cachedDate;
        private XDocument? cachedXml;
        public TCMBKurService() { }
        private async Task<XDocument> GetXmlAsync(DateOnly? date, CancellationToken cancellationToken)
        {
            if (this.cachedXml != null && this.cachedDate == date) { return this.cachedXml; }
            var dataTuple = await this.GetUrl(date).GetBinaryDataAsync(TimeSpan.FromSeconds(5), cancellationToken);
            if (dataTuple.haserror) { throw dataTuple.ex; }
            this.cachedXml = XDocument.Parse(Encoding.UTF8.GetString(dataTuple.databinary));
            this.cachedDate = date;
            return cachedXml;
        }
        private Uri GetUrl(DateOnly? date) => new((!date.HasValue || date.Value == DateTime.Today.ToDateOnly()) ? "https://www.tcmb.gov.tr/kurlar/today.xml" : $"https://www.tcmb.gov.tr/kurlar/{date:yyyyMM}/{date:ddMMyyyy}.xml");
        private TCMBKurDto GetRate(XDocument xml, string code)
        {
            var node = xml.Descendants("Currency").FirstOrDefault(x => x.Attribute("CurrencyCode")?.Value == code);
            if (node == null) { throw new Exception($"Kur bilgisi alınamadı: \"{code}\""); }
            var data = new TCMBKurDto();
            if (Int32.TryParse(node.Element(nameof(data.Unit))?.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out int _unit)) { data.Unit = _unit; }
            if (Decimal.TryParse(node.Element(nameof(data.ForexBuying))?.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal _value)) { data.ForexBuying = _value; }
            if (Decimal.TryParse(node.Element(nameof(data.ForexSelling))?.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out _value)) { data.ForexSelling = _value; }
            if (Decimal.TryParse(node.Element(nameof(data.BanknoteBuying))?.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out _value)) { data.BanknoteBuying = _value; }
            if (Decimal.TryParse(node.Element(nameof(data.BanknoteSelling))?.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out _value)) { data.BanknoteSelling = _value; }
            if (data.Equals(new())) { throw new Exception($"Kur bilgisi alınamadı: \"{code}\""); }
            return data;
        }
        public async Task<TCMBKurDto> GetAsync(TCMBCurrencyCodeTypes type, DateOnly? date = null, CancellationToken cancellationToken = default) => this.GetRate(await this.GetXmlAsync(date, cancellationToken), type.ToString("g"));
        public Task<TCMBKurDto> GetUSDAsync(DateOnly? date = null, CancellationToken cancellationToken = default) => this.GetAsync(TCMBCurrencyCodeTypes.USD, date, cancellationToken);
        public Task<TCMBKurDto> GetEURAsync(DateOnly? date = null, CancellationToken cancellationToken = default) => this.GetAsync(TCMBCurrencyCodeTypes.EUR, date, cancellationToken);
    }
}
