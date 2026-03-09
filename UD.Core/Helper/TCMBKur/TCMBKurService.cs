namespace UD.Core.Helper.TCMBKur
{
    using System;
    using System.Globalization;
    using System.Text;
    using System.Xml.Linq;
    using UD.Core.Extensions;
    public interface ITCMBKurService // AddSingleton
    {
        Task<TCMBKurResponse> Get(TCMBKurCodeTypes type, DateOnly? date = null, CancellationToken cancellationToken = default);
        Task<TCMBKurResponse> GetUSD(DateOnly? date = null, CancellationToken cancellationToken = default);
        Task<TCMBKurResponse> GetEUR(DateOnly? date = null, CancellationToken cancellationToken = default);
    }
    public sealed class TCMBKurService : ITCMBKurService
    {
        private DateOnly? cachedDate;
        private XDocument? cachedXml;
        public TCMBKurService() { }
        private async Task<XDocument> GetXml(DateOnly? date, CancellationToken cancellationToken)
        {
            if (this.cachedXml != null && this.cachedDate == date) { return this.cachedXml; }
            var dataTuple = await this.GetUrl(date).GetBinaryData(TimeSpan.FromSeconds(5), cancellationToken);
            if (dataTuple.hasError) { throw dataTuple.ex; }
            this.cachedXml = XDocument.Parse(Encoding.UTF8.GetString(dataTuple.dataBinary));
            this.cachedDate = date;
            return cachedXml;
        }
        private Uri GetUrl(DateOnly? date) => new((!date.HasValue || date.Value == DateTime.Today.ToDateOnly()) ? "https://www.tcmb.gov.tr/kurlar/today.xml" : $"https://www.tcmb.gov.tr/kurlar/{date:yyyyMM}/{date:ddMMyyyy}.xml");
        private TCMBKurResponse GetRate(XDocument xml, string code)
        {
            var node = xml.Descendants("Currency").FirstOrDefault(x => x.Attribute("CurrencyCode")?.Value == code);
            if (node == null) { throw new Exception($"Kur bilgisi alınamadı: \"{code}\""); }
            var data = new TCMBKurResponse();
            if (Int32.TryParse(node.Element(nameof(data.Unit))?.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out int _unit)) { data.Unit = _unit; }
            if (Decimal.TryParse(node.Element(nameof(data.ForexBuying))?.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal _value)) { data.ForexBuying = _value; }
            if (Decimal.TryParse(node.Element(nameof(data.ForexSelling))?.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out _value)) { data.ForexSelling = _value; }
            if (Decimal.TryParse(node.Element(nameof(data.BanknoteBuying))?.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out _value)) { data.BanknoteBuying = _value; }
            if (Decimal.TryParse(node.Element(nameof(data.BanknoteSelling))?.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out _value)) { data.BanknoteSelling = _value; }
            if (data.Equals(new())) { throw new Exception($"Kur bilgisi alınamadı: \"{code}\""); }
            return data;
        }
        public async Task<TCMBKurResponse> Get(TCMBKurCodeTypes type, DateOnly? date = null, CancellationToken cancellationToken = default) => this.GetRate(await this.GetXml(date, cancellationToken), type.ToString("g"));
        public Task<TCMBKurResponse> GetUSD(DateOnly? date = null, CancellationToken cancellationToken = default) => this.Get(TCMBKurCodeTypes.USD, date, cancellationToken);
        public Task<TCMBKurResponse> GetEUR(DateOnly? date = null, CancellationToken cancellationToken = default) => this.Get(TCMBKurCodeTypes.EUR, date, cancellationToken);
    }
}
