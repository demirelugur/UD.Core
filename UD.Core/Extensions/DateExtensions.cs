namespace UD.Core.Extensions
{
    using System;
    using System.Globalization;
    using static UD.Core.Helper.GlobalConstants;
    public static class DateExtensions
    {
        /// <summary>Hafta içi günlerini (Pazartesi, Salı, Çarşamba, Perşembe, Cuma) kontrol eder. Eğer belirtilen <see cref="DayOfWeek"/> değeri bu günlerden biri ise <c>true</c>, aksi halde <c>false</c> döner.</summary>
        public static bool IsWeekDays(this DayOfWeek dayOfWeek) => dayOfWeek.Includes(DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday);
        /// <summary>Verilen <see cref="DayOfWeek"/> değerinin, belirtilen dile göre gün adını döndürür.</summary>
        /// <param name="dayOfWeek">Gün bilgisi.</param>
        /// <param name="isElongated">><see langword="true"/> ise tam gün adı, ><see langword="false"/> ise kısaltılmış gün adı döndürülür.</param>
        /// <returns>Belirtilen dile göre gün adı.</returns>
        public static string GetDayName(this DayOfWeek dayOfWeek, bool isElongated = true)
        {
            var dtf = CultureInfo.CurrentUICulture.DateTimeFormat;
            return isElongated ? dtf.GetDayName(dayOfWeek) : dtf.GetAbbreviatedDayName(dayOfWeek);
        }
        /// <summary>Verilen <see cref="DateTime"/> değerinin ay adını, belirtilen dile göre döndürür.</summary>
        /// <param name="dateTime">Tarih bilgisi.</param>
        /// <param name="isElongated">><see langword="true"/> ise tam ay adı, ><see langword="false"/> ise kısaltılmış ay adı döndürülür.</param>
        /// <returns>Belirtilen dile göre ay adı.</returns>
        public static string GetMonthName(this DateTime dateTime, bool isElongated = true)
        {
            var dtf = CultureInfo.CurrentUICulture.DateTimeFormat;
            return isElongated ? dtf.GetMonthName(dateTime.Month) : dtf.GetAbbreviatedMonthName(dateTime.Month);
        }
        /// <summary>Belirtilen <see cref="DateTime"/> nesnesini yalnızca tarih bilgisini içeren bir <see cref="DateOnly"/> nesnesine dönüştürür.</summary>
        /// <param name="dateTime">Dönüştürülecek <see cref="DateTime"/> nesnesi.</param>
        /// <returns>Yalnızca tarih bilgisini içeren bir <see cref="DateOnly"/> nesnesi.</returns>
        public static DateOnly ToDateOnly(this DateTime dateTime) => DateOnly.FromDateTime(dateTime);
        /// <summary>Verilen DateTime nesnesini OADate tamsayı biçimine dönüştürür.</summary>
        /// <param name="dateTime">Dönüştürülecek tarih/saat.</param>
        /// <returns>OADate tamsayı değeri.</returns>
        public static int ToOADateInteger(this DateTime dateTime) => Convert.ToInt32(dateTime.Date.ToOADate());
        /// <summary>Verilen DateTime nesnesini ISO 8601 biçiminde dizeye dönüştürür.</summary>
        /// <param name="dateTime">Dönüştürülecek tarih/saat.</param>
        /// <returns>ISO 8601 biçiminde dize.</returns>
        public static string ToISO8601(this DateTime dateTime) => dateTime.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
        /// <summary>Gece yarısından (00:00:00.000) itibaren geçen toplam milisaniyeyi döner. Alabileceği maksimum değer: 86399999</summary>
        public static int ToMillisecondsSinceMidnight(this DateTime dateTime) => (dateTime.ToSecondsSinceMidnight() * 1000) + dateTime.Millisecond;
        /// <summary>Gece yarısından (00:00:00) itibaren geçen toplam saniyeyi döner. Alabileceği maksimum değer: 86399</summary>
        public static int ToSecondsSinceMidnight(this DateTime dateTime) => (dateTime.ToMinutesSinceMidnight() * 60) + dateTime.Second;
        /// <summary>Gece yarısından (00:00) itibaren geçen toplam dakikayı döner. Alabileceği maksimum değer: 1439</summary>
        public static int ToMinutesSinceMidnight(this DateTime dateTime) => (dateTime.Hour * 60) + dateTime.Minute;
        /// <summary>Verilen DateTime nesnesini Unix zaman damgası (milisaniye) biçiminde döndürür.</summary>
        /// <param name="dateTime">Dönüştürülecek tarih/saat.</param>
        /// <returns>Unix zaman damgası milisaniye değeri.</returns>
        public static long ToUnixTimestampMilliseconds(this DateTime dateTime) => ((dateTime.ToUniversalTime().Ticks - DateTime.UnixEpoch.Ticks) / TimeSpan.TicksPerMillisecond);
        /// <summary>Verilen DateTime nesnesinin ait olduğu ayın ilk gününü döndürür.</summary>
        /// <param name="dateTime">İlgili tarih.</param>
        /// <returns>Ayın ilk günü.</returns>
        public static DateTime GetFirstDayOfMonth(this DateTime dateTime) => new(dateTime.Year, dateTime.Month, 1);
        /// <summary>Verilen DateTime nesnesinin ait olduğu ayın son gününü döndürür.</summary>
        /// <param name="dateTime">İlgili tarih.</param>
        /// <returns>Ayın son günü.</returns>
        public static DateTime GetLastDayOfMonth(this DateTime dateTime) => new(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month));
        /// <summary>Belirtilen tarihin gün sonunu (23:59:59.9999999) döndürür.</summary>
        /// <param name="dateTime">Gün sonu hesaplanacak tarih ve saat değeri. </param>
        /// <returns>Belirtilen günün son anını temsil eden DateTime değeri.</returns>
        public static DateTime EndOfDay(this DateTime dateTime) => dateTime.Date.AddDays(1).AddTicks(-1);
        /// <summary>Belirtilen tarihten sonraki ilk iş gününü döndürür. Cumartesi ve Pazar günleri atlanarak bir sonraki hafta içi güne geçer.</summary>
        /// <param name="dateTime">Başlangıç tarihi.</param>
        /// <returns>Bir sonraki iş günü (Pazartesi - Cuma arası) olan DateTime değeri.</returns>
        public static DateTime NextWorkDay(this DateTime dateTime)
        {
            var next = dateTime.AddDays(1);
            while (!next.DayOfWeek.IsWeekDays()) { next = next.AddDays(1); }
            return next;
        }
        /// <summary>Belirtilen tarihten önceki ilk iş gününü döndürür. Cumartesi ve Pazar günleri atlanarak bir önceki hafta içi güne geçer.</summary>
        /// <param name="dateTime">Başlangıç tarihi.</param>
        /// <returns>Bir önceki iş günü (Pazartesi - Cuma arası) olan DateTime değeri.</returns>
        public static DateTime PreviousWorkDay(this DateTime dateTime)
        {
            var previous = dateTime.AddDays(-1);
            while (!previous.DayOfWeek.IsWeekDays()) { previous = previous.AddDays(-1); }
            return previous;
        }
        /// <summary>Verilen <see cref="DateTime"/> değerinin SQL Server&#39;ın kabul ettiği minimum tarihten (1753-01-01) küçük olup olmadığını kontrol eder. Eğer tarih SQL minimumu olan 1753-01-01 (saat 00:00:00) veya daha büyükse aynı <see cref="DateTime"/> değerini döner; aksi halde <c>null</c> döner.</summary>
        public static DateTime? SafeSqlDateTime(this DateTime dateTime)
        {
            if (dateTime >= DateConstants.SqlMinValue.ToDateTime(default)) { return dateTime; }
            return null;
        }
    }
}