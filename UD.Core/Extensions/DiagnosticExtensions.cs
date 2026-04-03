namespace UD.Core.Extensions
{
    using System;
    using System.Diagnostics;
    using UD.Core.Helper.Validation;
    public static class DiagnosticExtensions
    {
        /// <summary>Stopwatch&#39;ı durdurur ve geçen süreyi döner.</summary>
        /// <param name="stopWatch">Zamanlayıcı nesnesi.</param>
        /// <returns>Durdurulduktan sonra geçen süre.</returns>
        public static TimeSpan StopThenGetElapsed(this Stopwatch stopWatch)
        {
            Guard.ThrowIfNull(stopWatch, nameof(stopWatch));
            stopWatch.Stop();
            return stopWatch.Elapsed;
        }
    }
}