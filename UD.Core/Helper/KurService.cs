namespace UD.Core.Helper
{
    using System;
    using System.Globalization;
    using System.Text;
    using System.Xml.Linq;
    using UD.Core.Extensions;
    public interface IKurService
    {
        Task<decimal> GetUsdAsync(DateOnly? date = null, CancellationToken cancellationToken = default);
        Task<decimal> GetEuroAsync(DateOnly? date = null, CancellationToken cancellationToken = default);
    }
    public class KurService : IKurService
    {
        private DateOnly? cachedDate;
        private XDocument? cachedXml;
        public KurService() { }
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
        private decimal GetRate(XDocument xml, string code)
        {
            var node = xml.Descendants("Currency").FirstOrDefault(x => x.Attribute("CurrencyCode")?.Value == code);
            var value = (node == null ? "" : node.Element("ForexSelling")?.Value);
            if (Decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal result)) { return result; }
            throw new Exception($"Kur bilgisi alınamadı: \"{code}\"");
        }
        public async Task<decimal> GetUsdAsync(DateOnly? date = null, CancellationToken cancellationToken = default) => this.GetRate(await this.GetXmlAsync(date, cancellationToken), "USD");
        public async Task<decimal> GetEuroAsync(DateOnly? date = null, CancellationToken cancellationToken = default) => this.GetRate(await this.GetXmlAsync(date, cancellationToken), "EUR");
    }
}