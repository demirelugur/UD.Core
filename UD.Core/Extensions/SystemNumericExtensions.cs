namespace UD.Core.Extensions
{
    using System;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using System.Security.Cryptography;
    using UD.Core.Helper;
    using UD.Core.Helper.Validation;
    using static UD.Core.Helper.GlobalConstants;
    public static class SystemNumericExtensions
    {
        #region Long, Ulong
        /// <summary>Unix zaman damgasını, yerel bir <see cref="DateTime"/> değerine dönüştürür.</summary>
        /// <param name="getTime">Unix zaman damgası (milisaniye cinsinden).</param>
        /// <returns>Dönüştürülen yerel <see cref="DateTime"/> değeri.</returns>
        public static DateTime ToJsDate(this long getTime) => DateTime.UnixEpoch.AddMilliseconds(Convert.ToDouble(getTime)).ToLocalTime();
        /// <summary>Active Directory&#39;de kullanılan FILETIME biçimindeki bir değeri (1 Ocak 1601&#39;den itibaren 100 nanosaniye cinsinden tick) UTC zaman diliminde bir DateTime nesnesine çevirir. <para>Eğer filetime değeri 0 veya <see cref="Int64.MaxValue"/> ise, hesap süresiz kabul edilir ve <see cref="DateTime.MaxValue"/> döndürülür. Geçersiz bir filetime değeri durumunda null döner.</para></summary>
        /// <param name="fileTime">Çevrilecek 64 bitlik FILETIME değeri.</param>
        /// <returns>Başarılı olursa DateTime nesnesi, süresiz hesaplar için DateTime.MaxValue, geçersiz değerler için null.</returns>
        public static DateTime? ToFileTimeUTC(this long fileTime)
        {
            if (fileTime.Includes(0, Int64.MaxValue)) { return DateTime.MaxValue; }
            try { return DateTime.FromFileTimeUtc(fileTime); }
            catch { return null; }
        }
        /// <summary>Verilen değerin geçerli bir T.C. Kimlik Numarası olup olmadığını kontrol eder.
        /// <para>
        /// Doğrulama adımları:
        /// <list type="bullet">
        /// <item><description>11 haneli olmalıdır.</description></item>
        /// <item><description>Sadece rakamlardan oluşmalıdır.</description></item>
        /// <item><description>İlk hanesi &#39;0&#39; olamaz.</description></item>
        /// <item><description>10. hane, belirli aritmetik kurala göre (tek haneler ve çift haneler toplamı) kontrol edilir.</description></item>
        /// <item><description>11. hane, ilk 10 hanenin toplamının 10&#39;a bölümünden kalan ile doğrulanır.</description></item>
        /// </list>
        /// </para>
        /// </summary>
        /// <param name="tckn">Geçerliliği kontrol edilecek T.C. Kimlik Numarası.</param>
        /// <returns>Geçerli bir T.C. Kimlik Numarası ise <see langword="true"/>, aksi durumda <see langword="false"/> döner.</returns>
        public static bool IsTCKimlikNo(this long tckn)
        {
            var r = false;
            var tcknString = (tckn > 0 ? tckn.ToString() : "");
            if (tcknString.Length == MaximumLengthConstants.Tckn)
            {
                var t = tcknString.ToCharArray().Select(x => Convert.ToInt32(Convert.ToString(x))).ToArray();
                r = ((((t[0] + t[2] + t[4] + t[6] + t[8]) * 7) - (t[1] + t[3] + t[5] + t[7])) % 10) == t[9] && (t.Take(10).Sum() % 10) == t[10];
            }
            return r;
        }
        /// <summary>Verilen sayının geçerli bir T.C. Vergi Kimlik Numarası (VKN) olup olmadığını kontrol eder.
        /// <para>
        /// VKN doğrulama algoritması şu adımlarla çalışır:
        /// <list type="bullet">
        ///   <item><description>Eğer sayı 10 haneli değilse, başına sıfır(lar) eklenerek 10 haneye tamamlanır.</description></item>
        ///   <item><description>10 haneli VKN için her haneye özel matematiksel işlem uygulanır.</description></item>
        ///   <item><description>İlk 9 haneden elde edilen ara sonuçların toplamı hesaplanır.</description></item>
        ///   <item><description>Son hanenin (kontrol basamağı) doğruluğu algoritmaya göre kontrol edilir.</description></item>
        /// </list>
        /// </para>
        /// </summary>
        /// <param name="vkn">Doğrulanacak T.C. Vergi Kimlik Numarası</param>
        /// <returns>VKN geçerliyse <see langword="true"/>, değilse <see langword="false"/> döner.</returns>
        public static bool IsVergiKimlikNo(this long vkn)
        {
            var r = false;
            if (vkn > 0)
            {
                var vknString = vkn.ToString();
                if (vknString.Length < MaximumLengthConstants.Vkn) { vknString = vknString.Replicate(MaximumLengthConstants.Vkn, '0', true); } // 33583636 (8 Rakam) -> 0033583636, 602883151 (9 Rakam) -> 0602883151
                if (vknString.Length == MaximumLengthConstants.Vkn)
                {
                    int i, t;
                    var numbers = new int[9];
                    for (i = 0; i < 9; i++)
                    {
                        t = (Convert.ToInt32(vknString[i].ToString()) + (9 - i)) % 10;
                        numbers[i] = (t * Convert.ToInt32(Math.Pow(2, (9 - i)))) % 9;
                        if (t != 0 && numbers[i] == 0) { numbers[i] = 9; }
                    }
                    r = (((10 - ((numbers.Sum() % 10) % 10)) % 10) == Convert.ToInt32(vknString[9].ToString()));
                }
            }
            return r;
        }
        /// <summary><paramref name="value"/> değeri T.C. Kimlik Numarası (TCK) veya Vergi Kimlik Numarası (VKN) ise <see langword="true"/> döner.</summary>
        public static bool IsTCKNorVKN(this long value) => (value.IsTCKimlikNo() || value.IsVergiKimlikNo());
        /// <summary>Belirtilen uzunlukta, kriptografik olarak güvenli rastgele bayt dizisi (anahtar) üretir. </summary>
        /// <param name="length">Üretilecek anahtarın bayt cinsinden uzunluğu.</param>
        /// <returns>Rastgele üretilmiş baytlardan oluşan anahtar dizisi.</returns>
        public static byte[] GenerateRandomKey(this long length)
        {
            Guard.ThrowIfZeroOrNegative(length, nameof(length));
            using (var rng = RandomNumberGenerator.Create())
            {
                var byteArray = new byte[length];
                rng.GetBytes(byteArray);
                return byteArray;
            }
        }
        /// <summary>Verilen sayının asal olup olmadığını kontrol eder.</summary>
        /// <param name="value">Kontrol edilecek pozitif tamsayı.</param>
        /// <returns>Asal ise <see langword="true"/>, değilse <see langword="false"/> döner.</returns>
        public static bool IsPrimeNumber(this ulong value)
        {
            if (value < 2) { return false; }
            if (value == 2) { return true; }
            if ((value % 2) == 0) { return false; }
            if (value == 5) { return true; }
            if ((value % 5) == 0) { return false; }
            ulong i, limit = Convert.ToUInt64(Math.Sqrt(value));
            for (i = 3; i <= limit; i += 2) { if ((value % i) == 0) { return false; } }
            return true;
        }
        #endregion
        #region Decimal
        /// <summary>Bir decimal değeri, InvariantCulture kullanarak string&#39;e dönüştürür.</summary>
        /// <param name="value">Dönüştürülecek <see cref="Decimal"/> değer.</param>
        /// <returns>Decimal değerin string temsilini döner.</returns>
        /// <remarks>Bu metodun ters işlemi için <code>Convert.ToDecimal(value, CultureInfo.InvariantCulture);</code> kullanılabilir.</remarks>
        public static string ToStringInvariantCulture(this decimal value) => value.ToString(CultureInfo.InvariantCulture);
        /// <summary>Verilen bir ondalık değeri belirtilen ondalık basamak sayısına yuvarlayarak string olarak döndürür. İsteğe bağlı olarak, yuvarlama yöntemi belirtilebilir.</summary>
        /// <param name="d">Yuvarlanacak <see cref="Decimal"/> değeri.</param>
        /// <param name="decimals">Yuvarlanacak ondalık basamak sayısı (varsayılan olarak 2).</param>
        /// <param name="midpointRounding">Yuvarlama yöntemi (varsayılan olarak <see cref="MidpointRounding.AwayFromZero"/>).</param>
        /// <returns>Yuvarlanmış değerin string temsili.</returns>
        public static string ToRound(this decimal d, int decimals = 2, MidpointRounding midpointRounding = MidpointRounding.AwayFromZero) => Decimal.Round(d, decimals, midpointRounding).ToString();
        #endregion
        #region Double
        /// <summary>Dosya boyutu gibi büyük sayıları insan tarafından okunabilir bir biçimde formatlar. Örneğin, 1536 değeri &quot;1.5 KB&quot; olarak dönecektir.</summary>
        public static string ToFileSizeString(this double value)
        {
            value = Math.Max(0, value);
            int j = 0, sz = ArrayConstants.FileSizeUnits.Length - 1;
            while (value > 1024 && j < sz) { value /= 1024; j++; }
            return String.Join(" ", (Math.Ceiling(value * 100) / 100).ToString(), ArrayConstants.FileSizeUnits[j]);
        }
        #endregion
        #region Byte
        /// <summary>SQL Server&#39;ın sistem tür kimliğini <c>([system_type_id])</c> <see cref="SqlDbType"/> enum değerine dönüştürür.</summary>
        /// <param name="systemTypeId">SQL Server [sys].[types] tablosundaki [system_type_id] değeri.</param>
        /// <returns>Eşleşen <see cref="SqlDbType"/> enum değeri.</returns>
        /// <exception cref="NotSupportedException">Geçersiz veya desteklenmeyen bir sistem tür kimliği verildiğinde fırlatılır.</exception>

        public static SqlDbType ToSqlDbType(this byte systemTypeId)
        {
            return systemTypeId switch
            {
                34 => SqlDbType.Image,
                35 => SqlDbType.Text,
                36 => SqlDbType.UniqueIdentifier,
                40 => SqlDbType.Date,
                41 => SqlDbType.Time,
                42 => SqlDbType.DateTime2,
                43 => SqlDbType.DateTimeOffset,
                48 => SqlDbType.TinyInt,
                52 => SqlDbType.SmallInt,
                56 => SqlDbType.Int,
                58 => SqlDbType.SmallDateTime,
                59 => SqlDbType.Real,
                60 => SqlDbType.Money,
                61 => SqlDbType.DateTime,
                62 => SqlDbType.Float,
                99 => SqlDbType.NText,
                104 => SqlDbType.Bit,
                106 => SqlDbType.Decimal,
                122 => SqlDbType.SmallMoney,
                127 => SqlDbType.BigInt,
                165 => SqlDbType.VarBinary,
                167 => SqlDbType.VarChar,
                173 => SqlDbType.Binary,
                175 => SqlDbType.Char,
                189 => SqlDbType.Timestamp,
                231 => SqlDbType.NVarChar,
                239 => SqlDbType.NChar,
                241 => SqlDbType.Xml,
                _ => throw new NotSupportedException(Checks.IsEnglishCurrentUICulture ? $"Invalid or unsupported {nameof(systemTypeId)}: {systemTypeId}" : $"Geçersiz veya desteklenmeyen {nameof(systemTypeId)}: {systemTypeId}"),
            };
        }
        #endregion
    }
}