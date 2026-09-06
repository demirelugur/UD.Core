namespace UD.Core.Results
{
    using UD.Core.Helper;
    public sealed class DateIntervalResult
    {
        private readonly int _year;
        private readonly int _month;
        private readonly int _day;
        public DateIntervalResult() : this(default, default, default) { }
        public DateIntervalResult(int year, int month, int day)
        {
            this._year = year;
            this._month = month;
            this._day = day;
        }
        public override string ToString()
        {
            var r = new List<string>();
            if (this._year > 0)
            {
                var p0 = Checks.IsEnglishCurrentUICulture ? "year" : "yıl";
                r.Add(String.Join(" ", this._year.ToString(), p0));
            }
            if (this._month > 0)
            {
                var p1 = Checks.IsEnglishCurrentUICulture ? "month" : "ay";
                r.Add(String.Join(" ", this._month.ToString(), p1));
            }
            if (this._day > 0)
            {
                var p2 = Checks.IsEnglishCurrentUICulture ? "day" : "gün";
                r.Add(String.Join(" ", this._day.ToString(), p2));
            }
            return (r.Count > 0 ? String.Join(", ", r) : "");
        }
    }
}