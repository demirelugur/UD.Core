using System.Diagnostics;
namespace UD.Core.Extensions
{
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