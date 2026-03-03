namespace UD.Core.Helper.Results
{
    using UD.Core.Helper.Validation;
    using static UD.Core.Helper.OrtakTools;
    public sealed class DateDiffResult
    {
        private readonly DateTime basdate;
        private readonly DateTime bitdate;
        public DateDiffResult(object basdate, object bitdate)
        {
            this.basdate = _to.ToDateTimeFromObject(basdate, default);
            this.bitdate = (bitdate == null ? DateTime.Today : _to.ToDateTimeFromObject(bitdate, default));
        }
        public (int yil, int ay, int gun, TimeSpan ts) CalculateDateDifference()
        {
            if (this.basdate == this.bitdate || Math.Abs(this.bitdate.Ticks - this.basdate.Ticks) < TimeSpan.TicksPerMillisecond) { return (0, 0, 0, TimeSpan.Zero); }
            int yil = this.bitdate.Year - this.basdate.Year, ay = this.bitdate.Month - this.basdate.Month, gun = this.bitdate.Day - this.basdate.Day;
            var ts = (this.bitdate.TimeOfDay - this.basdate.TimeOfDay);
            var tsisnegative = ts < TimeSpan.Zero;
            if (tsisnegative) { gun--; }
            if (gun < 0)
            {
                if (yil == 0 && ay == 0) { gun = 0; }
                else
                {
                    ay--;
                    gun = (DateTime.DaysInMonth(this.basdate.Year, this.basdate.Month) - this.basdate.Day + this.bitdate.Day);
                }
            }
            if (ay < 0)
            {
                yil--;
                ay = 12 + ay;
            }
            return (yil, ay, gun, ((this.bitdate > this.basdate && tsisnegative) ? new TimeSpan(TimeSpan.TicksPerDay - ts.Negate().Ticks) : ts));
        }
        public string FormatDateDifference(string p0 = "yıl", string p1 = "ay", string p2 = "gün")
        {
            Guard.CheckEmpty(p0, nameof(p0));
            Guard.CheckEmpty(p1, nameof(p1));
            Guard.CheckEmpty(p2, nameof(p2));
            var r = new List<string>();
            var v = this.CalculateDateDifference();
            if (v.yil > 0) { r.Add(String.Join(" ", v.yil.ToString(), p0)); }
            if (v.ay > 0) { r.Add(String.Join(" ", v.ay.ToString(), p1)); }
            if (v.gun > 0) { r.Add(String.Join(" ", v.gun.ToString(), p2)); }
            return (r.Count > 0 ? String.Join(", ", r) : "");
        }
    }
}