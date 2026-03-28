namespace UD.Core.Helper.Results
{
    using UD.Core.Extensions;
    using UD.Core.Helper;
    public sealed class DateDiffResult
    {
        private readonly DateTime basDate;
        private readonly DateTime bitDate;
        public DateDiffResult(object basDate, object bitDate)
        {
            this.basDate = Converters.ToDateTimeFromObject(basDate, default);
            this.bitDate = (bitDate == null ? DateTime.Today : Converters.ToDateTimeFromObject(bitDate, default));
        }
        public (int yil, int ay, int gun, TimeOnly ts) CalculateDateDifference()
        {
            if (this.basDate > this.bitDate)
            {
                if (Guards.IsEnglishDefaultThreadCurrentUICulture)  {  throw new ArgumentException("The start date must be a value before the end date!"); }
                throw new ArgumentException("Başlangıç tarihi, Bitiş Tarihinden önce bir değer olmalıdır!");
            }
            var ts = (this.bitDate - this.basDate).ToTimeOnly();
            var yil = this.bitDate.Year - this.basDate.Year;
            var ay = this.bitDate.Month - this.basDate.Month;
            var gun = this.bitDate.Day - this.basDate.Day;
            if (gun < 0)
            {
                ay--;
                gun += DateTime.DaysInMonth(this.bitDate.Year, this.bitDate.Month == 1 ? 12 : this.bitDate.Month - 1);
            }
            if (ay < 0)
            {
                yil--;
                ay += 12;
            }
            return (yil, ay, gun, ts);
        }
        public string FormatDateDifference()
        {
            var r = new List<string>();
            var (yil, ay, gun, _) = this.CalculateDateDifference();
            var p0 = Guards.IsEnglishDefaultThreadCurrentUICulture ? "year" : "yıl";
            var p1 = Guards.IsEnglishDefaultThreadCurrentUICulture ? "month" : "ay";
            var p2 = Guards.IsEnglishDefaultThreadCurrentUICulture ? "day" : "gün";
            if (yil > 0) { r.Add(String.Join(" ", yil.ToString(), p0)); }
            if (ay > 0) { r.Add(String.Join(" ", ay.ToString(), p1)); }
            if (gun > 0) { r.Add(String.Join(" ", gun.ToString(), p2)); }
            return (r.Count > 0 ? String.Join(", ", r) : "");
        }
    }
}
