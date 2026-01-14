namespace UD.Core.Extensions
{
    using System;
    using System.Globalization;
    using System.Linq;
    using static UD.Core.Helper.GlobalConstants;
    public static class NumericExtensions
    {
        /// <summary>
        /// Unix zaman damgasını, yerel bir <see cref="DateTime"/> değerine dönüştürür.
        /// </summary>
        /// <param name="gettime">Unix zaman damgası (milisaniye cinsinden).</param>
        /// <returns>Dönüştürülen yerel <see cref="DateTime"/> değeri.</returns>
        public static DateTime ToJsDate(this long gettime) => DateTime.UnixEpoch.AddMilliseconds(Convert.ToDouble(gettime)).ToLocalTime();
        /// <summary>
        /// Active Directory&#39;de kullanılan FILETIME biçimindeki bir değeri (1 Ocak 1601&#39;den itibaren 100 nanosaniye cinsinden tick) UTC zaman diliminde bir DateTime nesnesine çevirir. 
        /// <para>
        /// Eğer filetime değeri 0 veya <see cref="Int64.MaxValue"/> ise, hesap süresiz kabul edilir ve <see cref="DateTime.MaxValue"/> döndürülür. Geçersiz bir filetime değeri durumunda null döner.
        /// </para>
        /// </summary>
        /// <param name="filetime">Çevrilecek 64 bitlik FILETIME değeri.</param>
        /// <returns>Başarılı olursa DateTime nesnesi, süresiz hesaplar için DateTime.MaxValue, geçersiz değerler için null.</returns>
        public static DateTime? ToFileTimeUTC(this long filetime)
        {
            if (filetime.Includes(0, Int64.MaxValue)) { return DateTime.MaxValue; }
            try { return DateTime.FromFileTimeUtc(filetime); }
            catch { return null; }
        }
        /// <summary>
        /// Verilen değerin geçerli bir T.C. Kimlik Numarası olup olmadığını kontrol eder.
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
        /// <returns>
        /// Geçerli bir T.C. Kimlik Numarası ise <see langword="true"/>, aksi durumda <see langword="false"/> döner.
        /// </returns>
        public static bool IsTCKimlikNo(this long tckn)
        {
            var _r = false;
            if (tckn > 0)
            {
                var _tckn = tckn.ToString();
                if (_tckn.Length == _maximumlength.tckn)
                {
                    var _t = _tckn.ToCharArray().Select(x => Convert.ToInt32(Convert.ToString(x))).ToArray();
                    _r = ((((_t[0] + _t[2] + _t[4] + _t[6] + _t[8]) * 7) - (_t[1] + _t[3] + _t[5] + _t[7])) % 10) == _t[9] && (_t.Take(10).Sum() % 10) == _t[10];
                }
            }
            return _r;
        }
        /// <summary>
        /// Verilen sayının geçerli bir T.C. Vergi Kimlik Numarası (VKN) olup olmadığını kontrol eder.
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
            var _r = false;
            if (vkn > 0)
            {
                var _vkn = vkn.ToString();
                if (_vkn.Length < _maximumlength.vkn) { _vkn = _vkn.Replicate(_maximumlength.vkn, '0', 'l'); } // Not: 602883151(9 rakamlı) gibi değer gelirse başına 0602883151 yapabilmek için yazılmıştır
                if (_vkn.Length == _maximumlength.vkn)
                {
                    int i, t;
                    var _numbers = new int[9];
                    for (i = 0; i < 9; i++)
                    {
                        t = (Convert.ToInt32(_vkn[i].ToString()) + (9 - i)) % 10;
                        _numbers[i] = (t * Convert.ToInt32(Math.Pow(2, (9 - i)))) % 9;
                        if (t != 0 && _numbers[i] == 0) { _numbers[i] = 9; }
                    }
                    _r = (((10 - ((_numbers.Sum() % 10) % 10)) % 10) == Convert.ToInt32(_vkn[9].ToString()));
                }
            }
            return _r;
        }
        /// <summary>
        /// Verilen sayının asal olup olmadığını kontrol eder.
        /// </summary>
        /// <param name="value">Kontrol edilecek pozitif tamsayı.</param>
        /// <returns>Asal ise <see langword="true"/>, değilse <see langword="false"/> döner.</returns>
        public static bool IsPrimeNumber(this ulong value)
        {
            if (value < 2) { return false; }
            if (value == 2) { return true; }
            if ((value % 2) == 0) { return false; }
            if (value == 5) { return true; }
            if ((value % 5) == 0) { return false; }
            ulong _i, _limit = Convert.ToUInt64(Math.Sqrt(value));
            for (_i = 3; _i <= _limit; _i += 2) { if ((value % _i) == 0) { return false; } }
            return true;
        }
        /// <summary>
        /// Bir decimal değeri, InvariantCulture kullanarak string&#39;e dönüştürür.
        /// </summary>
        /// <param name="value">Dönüştürülecek <see cref="Decimal"/> değer.</param>
        /// <returns>Decimal değerin string temsilini döner.</returns>
        /// <remarks>Bu metodun ters işlemi için <code>Convert.ToDecimal(value, CultureInfo.InvariantCulture);</code> kullanılabilir.</remarks>
        public static string ToStringInvariantCulture(this decimal value) => value.ToString(CultureInfo.InvariantCulture);
        /// <summary>
        /// Verilen bir ondalık değeri belirtilen ondalık basamak sayısına yuvarlayarak string olarak döndürür. İsteğe bağlı olarak, yuvarlama yöntemi belirtilebilir.
        /// </summary>
        /// <param name="d">Yuvarlanacak <see cref="Decimal"/> değeri.</param>
        /// <param name="decimals">Yuvarlanacak ondalık basamak sayısı (varsayılan olarak 2).</param>
        /// <param name="midpointrounding">Yuvarlama yöntemi (varsayılan olarak <see cref="MidpointRounding.AwayFromZero"/>).</param>
        /// <returns>Yuvarlanmış değerin string temsili.</returns>
        public static string ToRound(this decimal d, int decimals = 2, MidpointRounding midpointrounding = MidpointRounding.AwayFromZero) => Decimal.Round(d, decimals, midpointrounding).ToString();
    }
}