namespace UD.Core.Extensions
{
    using System;
    using static UD.Core.Helper.GlobalConstants;
    public static class DateExtensions
    {
        /// <summary>Verilen <see cref="DateOnly"/> türündeki tarihi, SQL Server tarafından anlaşılabilir bir CONVERT(DATE, ...) ifadesine dönüştürür.</summary>
        /// <param name="date">SQL&#39;e dönüştürülecek tarih değeri.</param>
        /// <returns>Örnek çıktı: CONVERT(DATE, &#39;2000-01-01&#39;)</returns>
        public static string ToSqlExpressionDate(this DateOnly date) => $"CONVERT(DATE, '{date.ToString(_date.yyyyMMdd)}')";
        /// <summary>Belirtilen <see cref="DateTime"/> nesnesini yalnızca tarih bilgisini içeren bir <see cref="DateOnly"/> nesnesine dönüştürür.</summary>
        /// <param name="datetime">Dönüştürülecek <see cref="DateTime"/> nesnesi.</param>
        /// <returns>Yalnızca tarih bilgisini içeren bir <see cref="DateOnly"/> nesnesi.</returns>
        public static DateOnly ToDateOnly(this DateTime datetime) => DateOnly.FromDateTime(datetime);
        /// <summary>Verilen <see cref="DateTime"/> türündeki tarih ve saat bilgisini, SQL Server tarafından anlaşılabilir bir CONVERT(DATETIME, ...) ifadesine dönüştürür.</summary>
        /// <param name="datetime">SQL&#39;e dönüştürülecek tarih ve saat değeri.</param>
        /// <returns>Örnek çıktı: CONVERT(DATETIME, &#39;2025-10-16 09:45:32:123&#39;)</returns>
        public static string ToSqlExpressionDateTime(this DateTime datetime) => $"CONVERT(DATETIME, '{datetime.ToString("yyyy-MM-dd HH:mm:ss:fff")}')";
        /// <summary>Verilen DateTime nesnesini OADate tamsayı biçimine dönüştürür.</summary>
        /// <param name="datetime">Dönüştürülecek tarih/saat.</param>
        /// <returns>OADate tamsayı değeri.</returns>
        public static int ToOADateInteger(this DateTime datetime) => Convert.ToInt32(datetime.Date.ToOADate());
        /// <summary>Verilen DateTime nesnesini ISO 8601 biçiminde dizeye dönüştürür.</summary>
        /// <param name="datetime">Dönüştürülecek tarih/saat.</param>
        /// <returns>ISO 8601 biçiminde dize.</returns>
        public static string ToISO8601(this DateTime datetime) => datetime.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
        /// <summary>Gece yarısından (00:00:00.000) itibaren geçen toplam milisaniyeyi döner. Alabileceği maksimum değer: 86399999</summary>
        public static int ToMillisecondsSinceMidnight(this DateTime datetime) => (datetime.ToSecondsSinceMidnight() * 1000) + datetime.Millisecond;
        /// <summary>Gece yarısından (00:00:00) itibaren geçen toplam saniyeyi döner. Alabileceği maksimum değer: 86399</summary>
        public static int ToSecondsSinceMidnight(this DateTime datetime) => (datetime.ToMinutesSinceMidnight() * 60) + datetime.Second;
        /// <summary>Gece yarısından (00:00) itibaren geçen toplam dakikayı döner. Alabileceği maksimum değer: 1439</summary>
        public static int ToMinutesSinceMidnight(this DateTime datetime) => (datetime.Hour * 60) + datetime.Minute;
        /// <summary>Verilen DateTime nesnesini Unix zaman damgası (milisaniye) biçiminde döndürür.</summary>
        /// <param name="datetime">Dönüştürülecek tarih/saat.</param>
        /// <returns>Unix zaman damgası milisaniye değeri.</returns>
        public static long ToUnixTimestampMilliseconds(this DateTime datetime) => ((datetime.ToUniversalTime().Ticks - DateTime.UnixEpoch.Ticks) / TimeSpan.TicksPerMillisecond);
        /// <summary>Verilen DateTime nesnesinin ait olduğu ayın ilk gününü döndürür.</summary>
        /// <param name="datetime">İlgili tarih.</param>
        /// <returns>Ayın ilk günü.</returns>
        public static DateTime GetFirstDayOfMonth(this DateTime datetime) => new(datetime.Year, datetime.Month, 1);
        /// <summary>Verilen DateTime nesnesinin ait olduğu ayın son gününü döndürür.</summary>
        /// <param name="datetime">İlgili tarih.</param>
        /// <returns>Ayın son günü.</returns>
        public static DateTime GetLastDayOfMonth(this DateTime datetime) => new(datetime.Year, datetime.Month, DateTime.DaysInMonth(datetime.Year, datetime.Month));
        /// <summary>Belirtilen tarihin gün sonunu (23:59:59.9999999) döndürür.</summary>
        /// <param name="datetime">Gün sonu hesaplanacak tarih ve saat değeri. </param>
        /// <returns>Belirtilen günün son anını temsil eden DateTime değeri.</returns>
        public static DateTime EndOfDay(this DateTime datetime) => datetime.Date.AddDays(1).AddTicks(-1);
        /// <summary>Belirtilen tarihten sonraki ilk iş gününü döndürür. Cumartesi ve Pazar günleri atlanarak bir sonraki hafta içi güne geçer.</summary>
        /// <param name="datetime">Başlangıç tarihi.</param>
        /// <returns>Bir sonraki iş günü (Pazartesi - Cuma arası) olan DateTime değeri.</returns>
        public static DateTime NextWorkDay(this DateTime datetime)
        {
            var next = datetime.AddDays(1);
            while (next.DayOfWeek.Includes(DayOfWeek.Saturday, DayOfWeek.Sunday)) { next = next.AddDays(1); }
            return next;
        }
        /// <summary>
        /// Verilen <see cref="DateTime"/> değerinin SQL Server&#39;ın kabul ettiği minimum tarihten (1753-01-01) küçük olup olmadığını kontrol eder. Eğer tarih SQL minimumu olan 1753-01-01 (saat 00:00:00) veya daha büyükse aynı <see cref="DateTime"/> değerini döner; aksi halde <c>null</c> döner.
        /// </summary>
        public static DateTime? SafeSqlDateTimeMin(this DateTime datetime)
        {
            if (datetime >= _date.sqlMinValue.ToDateTime(default)) { return datetime; }
            return null;
        }
    }
}