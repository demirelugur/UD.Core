namespace UD.Core.Extensions
{
    using System;
    using System.Diagnostics;
    public static class DiagnosticsExtensions
    {
        /// <summary>Stopwatch&#39;ı durdurur ve geçen süreyi döner.</summary>
        /// <param name="stopWatch">Zamanlayıcı nesnesi.</param>
        /// <returns>Durdurulduktan sonra geçen süre.</returns>
        public static TimeSpan StopThenGetElapsed(this Stopwatch stopWatch)
        {
            stopWatch.Stop();
            return stopWatch.Elapsed;
        }
    }
}