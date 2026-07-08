namespace UD.Core.Helper.Results
{
    using UD.Core.Helper;
    public sealed class DateIntervalResult
    {
        public int year { get; set; }
        public int month { get; set; }
        public int day { get; set; }
        public DateIntervalResult() : this(default, default, default) { }
        public DateIntervalResult(int year, int month, int day)
        {
            this.year = year;
            this.month = month;
            this.day = day;
        }
        public override string ToString()
        {
            var r = new List<string>();
            if (this.year > 0)
            {
                var p0 = Checks.IsEnglishCurrentUICulture ? "year" : "yıl";
                r.Add(String.Join(" ", this.year.ToString(), p0));
            }
            if (this.month > 0)
            {
                var p1 = Checks.IsEnglishCurrentUICulture ? "month" : "ay";
                r.Add(String.Join(" ", this.month.ToString(), p1));
            }
            if (this.day > 0)
            {
                var p2 = Checks.IsEnglishCurrentUICulture ? "day" : "gün";
                r.Add(String.Join(" ", this.day.ToString(), p2));
            }
            return (r.Count > 0 ? String.Join(", ", r) : "");
        }
    }
}