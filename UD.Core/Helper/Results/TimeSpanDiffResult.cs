namespace UD.Core.Helper.Results
{
    using UD.Core.Extensions;
    using UD.Core.Helper;
    public sealed class TimeSpanDiffResult
    {
        private readonly TimeSpan timeSpan;
        public TimeSpanDiffResult(TimeSpan timeSpan)
        {
            this.timeSpan = timeSpan;
        }
        public TimeSpanDiffResult(TimeOnly timeOnly) : this(timeOnly.ToTimeSpan()) { }
        public (double totalHours, int minutes, int seconds, int milliseconds) DecomposeTimeSpan => (Math.Truncate(timeSpan.TotalHours), timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);
        public string FormatTimeSpan()
        {
            var p0 = Guards.IsEnglishDefaultThreadCurrentUICulture ? "sec." : "sn.";
            if (this.timeSpan == TimeSpan.Zero) { return $"0 {p0}"; }
            var (totalHours, minutes, seconds, milliseconds) = this.DecomposeTimeSpan;
            var r = new List<string>();
            var p1 = Guards.IsEnglishDefaultThreadCurrentUICulture ? "min." : "dk.";
            var p2 = Guards.IsEnglishDefaultThreadCurrentUICulture ? "hour" : "saat";
            if (totalHours != 0) { r.Add(String.Join(" ", Math.Abs(totalHours).ToString(), p2)); }
            if (minutes != 0) { r.Add(String.Join(" ", Math.Abs(minutes).ToString(), p1)); }
            if ((seconds > 0 && milliseconds > 0) || (seconds < 0 && milliseconds < 0)) { r.Add($"{Math.Abs(seconds)},{Math.Abs(milliseconds).ToString().Replicate(3)} {p0}"); }
            else if (seconds != 0 && milliseconds == 0) { r.Add(String.Join(" ", Math.Abs(seconds).ToString(), p0)); }
            else if (milliseconds != 0) { r.Add($"0,{Math.Abs(milliseconds).ToString().Replicate(3)} {p0}"); }
            return String.Join(" ", r);
        }
    }
}