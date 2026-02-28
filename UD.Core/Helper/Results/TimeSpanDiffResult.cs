namespace UD.Core.Helper.Results
{
    using UD.Core.Extensions;
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
            var _dts = this.DecomposeTimeSpan;
            var _r = new List<string>();
            if (_dts.totalhours != 0) { _r.Add(String.Join(" ", Math.Abs(_dts.totalhours).ToString(), p2)); }
            if (_dts.minutes != 0) { _r.Add(String.Join(" ", Math.Abs(_dts.minutes).ToString(), p1)); }
            if ((_dts.seconds > 0 && _dts.milliseconds > 0) || (_dts.seconds < 0 && _dts.milliseconds < 0)) { _r.Add($"{Math.Abs(_dts.seconds).ToString()},{Math.Abs(_dts.milliseconds).ToString().Replicate(3)} {p0}"); }
            else if (_dts.seconds != 0 && _dts.milliseconds == 0) { _r.Add(String.Join(" ", Math.Abs(_dts.seconds).ToString(), p0)); }
            else if (_dts.milliseconds != 0) { _r.Add($"0,{Math.Abs(_dts.milliseconds).ToString().Replicate(3)} {p0}"); }
            return String.Join(" ", _r);
        }
    }
}