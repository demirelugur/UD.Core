namespace UD.Core.Helper.Results
{
    using UD.Core.Helper.Validation;
    using static UD.Core.Helper.OrtakTools;
    public sealed class DateDiffResult
    {
        private readonly DateTime basDate;
        private readonly DateTime bitDate;
        public DateDiffResult(object basDate, object bitDate)
        {
            this.basDate = Converters.ToDateTimeFromObject(basDate, default);
            this.bitDate = (bitDate == null ? DateTime.Today : Converters.ToDateTimeFromObject(bitDate, default));
        }
        public (int yil, int ay, int gun, TimeSpan ts) CalculateDateDifference()
        {
            if (this.basDate == this.bitDate || Math.Abs(this.bitDate.Ticks - this.basDate.Ticks) < TimeSpan.TicksPerMillisecond) { return (0, 0, 0, TimeSpan.Zero); }
            int yil = this.bitDate.Year - this.basDate.Year, ay = this.bitDate.Month - this.basDate.Month, gun = this.bitDate.Day - this.basDate.Day;
            var ts = (this.bitDate.TimeOfDay - this.basDate.TimeOfDay);
            var isNegative = ts < TimeSpan.Zero;
            if (isNegative) { gun--; }
            if (gun < 0)
            {
                if (yil == 0 && ay == 0) { gun = 0; }
                else
                {
                    ay--;
                    gun = (DateTime.DaysInMonth(this.basDate.Year, this.basDate.Month) - this.basDate.Day + this.bitDate.Day);
                }
            }
            if (ay < 0)
            {
                yil--;
                ay = 12 + ay;
            }
            return (yil, ay, gun, ((this.bitDate > this.basDate && isNegative) ? new TimeSpan(TimeSpan.TicksPerDay - ts.Negate().Ticks) : ts));
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