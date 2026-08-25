namespace UD.Core.Helper.Results
{
    using UD.Core.Helper;
    public sealed class DateIntervalResult
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public DateIntervalResult() : this(default, default, default) { }
        public DateIntervalResult(int Year, int Month, int Day)
        {
            this.Year = Year;
            this.Month = Month;
            this.Day = Day;
        }
        public override string ToString()
        {
            var r = new List<string>();
            if (this.Year > 0)
            {
                var p0 = Checks.IsEnglishCurrentUICulture ? "year" : "yıl";
                r.Add(String.Join(" ", this.Year.ToString(), p0));
            }
            if (this.Month > 0)
            {
                var p1 = Checks.IsEnglishCurrentUICulture ? "month" : "ay";
                r.Add(String.Join(" ", this.Month.ToString(), p1));
            }
            if (this.Day > 0)
            {
                var p2 = Checks.IsEnglishCurrentUICulture ? "day" : "gün";
                r.Add(String.Join(" ", this.Day.ToString(), p2));
            }
            return (r.Count > 0 ? String.Join(", ", r) : "");
        }
    }
}