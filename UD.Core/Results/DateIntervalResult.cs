namespace UD.Core.Results
{
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
            if (this._year > 0) { r.Add(String.Concat(this._year.ToString(), " yıl")); }
            if (this._month > 0) { r.Add(String.Concat(this._month.ToString(), " ay")); }
            if (this._day > 0) { r.Add(String.Concat(this._day.ToString(), " gün")); }
            return (r.Count > 0 ? String.Join(", ", r) : "");
        }
    }
}