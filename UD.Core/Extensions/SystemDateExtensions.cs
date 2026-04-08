namespace UD.Core.Extensions
{
    using System;
    using System.Globalization;
    using UD.Core.Helper;
    using UD.Core.Helper.Results;
    using static UD.Core.Helper.GlobalConstants;
    public static class SystemDateExtensions
    {
        #region DateTime
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
            if (dateTime >= DateConstants.SqlServerMinValue.ToDateTime(default)) { return dateTime; }
            return null;
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
        /// <summary><paramref name="basDate"/> ile <paramref name="bitDate"/> arasındaki farkı yıl, ay, gün ve saat:dakika:saniye biçiminde hesaplar. Başlangıç tarihi bitiş tarihinden sonra ise hata fırlatır.</summary>
        public static DateIntervalResult GetDateInterval(this DateTime basDate, DateTime bitDate)
        {
            if (basDate > bitDate)
            {
                if (Checks.IsEnglishCurrentUICulture) { throw new ArgumentException("The start date must be a value before the end date!"); }
                throw new ArgumentException("Başlangıç tarihi, Bitiş Tarihinden önce bir değer olmalıdır!");
            }
            var ts = (bitDate - basDate).ToTimeOnly();
            var yil = bitDate.Year - basDate.Year;
            var ay = bitDate.Month - basDate.Month;
            var gun = bitDate.Day - basDate.Day;
            if (gun < 0)
            {
                ay--;
                gun += DateTime.DaysInMonth(bitDate.Year, bitDate.Month == 1 ? 12 : bitDate.Month - 1);
            }
            if (ay < 0)
            {
                yil--;
                ay += 12;
            }
            return new(yil, ay, gun, ts);
        }
        #endregion
        #region DayOfWeek
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
        #endregion
        #region TimeSpan
        /// <summary><paramref name="timeSpan"/> değerini, gece yarısından (00:00:00) itibaren geçen toplam süre olarak temsil eden bir <see cref="TimeOnly"/> nesnesine dönüştürür. Dönüşüm sırasında, <paramref name="timeSpan"/> değerinin gün kısmı dikkate alınmaz ve yalnızca saat, dakika, saniye ve milisaniye bilgisi kullanılır. Bu sayede, <paramref name="timeSpan"/> değeri 24 saatten büyük olsa bile, sonuç her zaman 00:00:00 ile 23:59:59.999 arasında bir zaman dilimini temsil eder.</summary>
        public static TimeOnly ToTimeOnly(this TimeSpan timeSpan) => TimeOnly.FromTimeSpan(timeSpan - TimeSpan.FromDays((int)timeSpan.TotalDays));
        /// <summary>TimeSpan değerini gün, saat, dakika ve saniye (milisaniye dahil) bileşenlerine ayırarak okunabilir bir metne dönüştürür. Negatif süreleri destekler.</summary>
        public static string ToPretty(this TimeSpan timeSpan)
        {
            var isEnglish = Checks.IsEnglishCurrentUICulture;
            var secondText = isEnglish ? "sec." : "sn.";
            if (timeSpan == TimeSpan.Zero) { return String.Concat("0 ", secondText); }
            var isNegative = timeSpan < TimeSpan.Zero;
            var ts = isNegative ? timeSpan.Duration() : timeSpan;
            var parts = new List<string>();
            if (ts.Days > 0)
            {
                var dayText = isEnglish ? (ts.Days > 1 ? "days" : "day") : "gün";
                parts.Add(String.Join(" ", ts.Days, dayText));
            }
            if (ts.Hours > 0)
            {
                var hourText = isEnglish ? (ts.Hours > 1 ? "hours" : "hour") : "saat";
                parts.Add(String.Join(" ", ts.Hours.ToString().Replicate(), hourText));
            }
            if (ts.Minutes > 0)
            {
                var minuteText = isEnglish ? "min." : "dk.";
                parts.Add(String.Join(" ", ts.Minutes.ToString().Replicate(), minuteText));
            }
            if (ts.Milliseconds > 0)
            {
                var nf = CultureInfo.CurrentUICulture.NumberFormat;
                parts.Add($"{(ts.Seconds > 0 ? ts.Seconds.ToString().Replicate() : "0")}{nf.CurrencyDecimalSeparator}{ts.Milliseconds.ToString().Replicate(3)} {secondText}");
            }
            else if (ts.Seconds > 0) { parts.Add(String.Join(" ", ts.Seconds.ToString().Replicate(), secondText)); }
            var result = String.Join(" ", parts);
            return isNegative ? String.Concat("- ", result) : result;
        }
        #endregion
    }
}