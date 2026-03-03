namespace UD.Core.Helper.Results
{
    using UD.Core.Extensions;
    using UD.Core.Helper.Validation;

    public sealed class TimeSpanDiffResult
    {
        private readonly TimeSpan timespan;
        public TimeSpanDiffResult(TimeSpan timespan)
        {
            this.timespan = timespan;
        }
        public TimeSpanDiffResult(TimeOnly timeonly) : this(timeonly.ToTimeSpan()) { }
        public (double totalhours, int minutes, int seconds, int milliseconds) DecomposeTimeSpan => (Math.Truncate(timespan.TotalHours), timespan.Minutes, timespan.Seconds, timespan.Milliseconds);
        public string FormatTimeSpan(string p0 = "sn.", string p1 = "dk.", string p2 = "saat")
        {
            Guard.CheckEmpty(p0, nameof(p0));
            if (this.timespan == TimeSpan.Zero) { return $"0 {p0}"; }
            Guard.CheckEmpty(p1, nameof(p1));
            Guard.CheckEmpty(p2, nameof(p2));
            var dts = this.DecomposeTimeSpan;
            var r = new List<string>();
            if (dts.totalhours != 0) { r.Add(String.Join(" ", Math.Abs(dts.totalhours).ToString(), p2)); }
            if (dts.minutes != 0) { r.Add(String.Join(" ", Math.Abs(dts.minutes).ToString(), p1)); }
            if ((dts.seconds > 0 && dts.milliseconds > 0) || (dts.seconds < 0 && dts.milliseconds < 0)) { r.Add($"{Math.Abs(dts.seconds).ToString()},{Math.Abs(dts.milliseconds).ToString().Replicate(3)} {p0}"); }
            else if (dts.seconds != 0 && dts.milliseconds == 0) { r.Add(String.Join(" ", Math.Abs(dts.seconds).ToString(), p0)); }
            else if (dts.milliseconds != 0) { r.Add($"0,{Math.Abs(dts.milliseconds).ToString().Replicate(3)} {p0}"); }
            return String.Join(" ", r);
        }
    }
}