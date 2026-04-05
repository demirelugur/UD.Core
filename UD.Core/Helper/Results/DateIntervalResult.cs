namespace UD.Core.Helper.Results
{
    using UD.Core.Helper;
    public sealed class DateIntervalResult
    {
        public int yil { get; set; }
        public int ay { get; set; }
        public int gun { get; set; }
        public TimeOnly ts { get; set; }
        public DateIntervalResult() : this(default, default, default, default) { }
        public DateIntervalResult(int yil, int ay, int gun, TimeOnly ts)
        {
            this.yil = yil;
            this.ay = ay;
            this.gun = gun;
            this.ts = ts;
        }
        public string ToPrettyString()
        {
            var r = new List<string>();
            if (this.yil > 0)
            {
                var p0 = ValidationChecks.IsEnglishCurrentUICulture ? "year" : "yıl";
                r.Add(String.Join(" ", this.yil.ToString(), p0));
            }
            if (this.ay > 0)
            {
                var p1 = ValidationChecks.IsEnglishCurrentUICulture ? "month" : "ay";
                r.Add(String.Join(" ", this.ay.ToString(), p1));
            }
            if (this.gun > 0)
            {
                var p2 = ValidationChecks.IsEnglishCurrentUICulture ? "day" : "gün";
                r.Add(String.Join(" ", this.gun.ToString(), p2));
            }
            return (r.Count > 0 ? String.Join(", ", r) : "");
        }
    }
}