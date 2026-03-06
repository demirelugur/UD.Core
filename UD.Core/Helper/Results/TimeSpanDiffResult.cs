namespace UD.Core.Helper.Results
{
    using UD.Core.Extensions;
    using UD.Core.Helper.Validation;
    public sealed class TimeSpanDiffResult
    {
        private readonly TimeSpan timeSpan;
        public TimeSpanDiffResult(TimeSpan timeSpan)
        {
            this.timeSpan = timeSpan;
        }
        public TimeSpanDiffResult(TimeOnly timeOnly) : this(timeOnly.ToTimeSpan()) { }
        public (double totalHours, int minutes, int seconds, int milliseconds) DecomposeTimeSpan => (Math.Truncate(timeSpan.TotalHours), timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);
        public string FormatTimeSpan(string p0 = "sn.", string p1 = "dk.", string p2 = "saat")
        {
            Guard.CheckEmpty(p0, nameof(p0));
            if (this.timeSpan == TimeSpan.Zero) { return $"0 {p0}"; }
            Guard.CheckEmpty(p1, nameof(p1));
            Guard.CheckEmpty(p2, nameof(p2));
            var dts = this.DecomposeTimeSpan;
            var r = new List<string>();
            if (dts.totalHours != 0) { r.Add(String.Join(" ", Math.Abs(dts.totalHours).ToString(), p2)); }
            if (dts.minutes != 0) { r.Add(String.Join(" ", Math.Abs(dts.minutes).ToString(), p1)); }
            if ((dts.seconds > 0 && dts.milliseconds > 0) || (dts.seconds < 0 && dts.milliseconds < 0)) { r.Add($"{Math.Abs(dts.seconds).ToString()},{Math.Abs(dts.milliseconds).ToString().Replicate(3)} {p0}"); }
            else if (dts.seconds != 0 && dts.milliseconds == 0) { r.Add(String.Join(" ", Math.Abs(dts.seconds).ToString(), p0)); }
            else if (dts.milliseconds != 0) { r.Add($"0,{Math.Abs(dts.milliseconds).ToString().Replicate(3)} {p0}"); }
            return String.Join(" ", r);
        }
    }
}