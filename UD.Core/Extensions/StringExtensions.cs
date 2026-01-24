namespace UD.Core.Extensions
{
    using Newtonsoft.Json.Linq;
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Net;
    using System.Net.Mail;
    using System.Numerics;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;
    using UD.Core.Helper;
    using static UD.Core.Helper.OrtakTools;
    public static class StringExtensions
    {
        /// <summary>
        /// Bir string&#39;i Guid&#39;e dönüştürür. String null veya geçersizse varsayılan Guid döner.
        /// </summary>
        /// <param name="value">Dönüştürülecek string.</param>
        /// <returns>Dönüştürülmüş Guid.</returns>
        public static Guid ToGuid(this string value) => value.ParseOrDefault<Guid>();
        /// <summary>
        /// Bir dizeyi <see cref="DateTime"/> türüne dönüştürür. Dize geçerli bir tarih formatında değilse, varsayılan <see cref="DateTime"/> değeri döndürülür.
        /// </summary>
        /// <param name="value">Dönüştürülecek tarih içeren dize.</param>
        /// <returns>Geçerli bir <see cref="DateTime"/> nesnesi veya varsayılan <see cref="DateTime"/> değeri.</returns>
        public static DateTime ToDate(this string value) => value.ParseOrDefault<DateTime>();
        private static readonly Dictionary<char, char> _charreplacements = new Dictionary<char, char>
        {
            { 'ş', 's' }, { 'Ş', 's' },
            { 'ö', 'o' }, { 'Ö', 'o' },
            { 'ü', 'u' }, { 'Ü', 'u' },
            { 'ç', 'c' }, { 'Ç', 'c' },
            { 'ğ', 'g' }, { 'Ğ', 'g' },
            { 'ı', 'i' }, { 'I', 'i' }, { 'İ', 'i' }
        };
        private static readonly char[] _charstoremove = new char[] { '?', '/', '.', '\'', '"', '#', '%', '&', '*', '!', '@', '+' };
        /// <summary>
        /// Verilen dizeyi SEO dostu bir hale getirir.
        /// </summary>
        /// <param name="value">Dönüştürülecek dize.</param>
        /// <returns>SEO dostu hale getirilmiş dize.</returns>
        public static string ToSeoFriendly(this string value)
        {
            value = value.ToStringOrEmpty();
            if (value == "") { return ""; }
            var _sb = new StringBuilder(value.Length);
            foreach (var item in value.ToCharArray())
            {
                if (_charreplacements.TryGetValue(item, out char _v)) { _sb.Append(_v); }
                else if (item == ' ') { _sb.Append('-'); }
                else if (Array.IndexOf(_charstoremove, item) == -1) { _sb.Append(item); }
            }
            value = _sb.ToString().ToLower().Trim();
            value = Regex.Replace(value, @"[^a-z0-9-]", "-");
            value = Regex.Replace(value, @"-+", "-");
            return value.Trim('-');
        }
        /// <summary>
        /// Verilen telefon numarasını Türk telefon biçime dönüştürür. Eğer telefon numarası geçerli bir Türk telefon numarası değilse, boş bir string döner.
        /// <para>Biçim: (###) ###-####</para>
        /// <para>Örneğin: &quot;5001112233&quot; girişi &quot;(500) 111-2233&quot; biçiminde döner.</para>
        /// </summary>
        /// <param name="phonenumberTR">Dönüştürülmek istenen telefon numarası.</param>
        /// <returns>Biçimlenmiş Türk telefon numarası ya da geçerli değilse boş bir string.</returns>
        public static string BeautifyPhoneNumberTR(this string phonenumberTR) => (_try.TryPhoneNumberTR(phonenumberTR, out string _s) ? $"({_s.Substring(0, 3)}) {_s.Substring(3, 3)}-{_s.Substring(6, 4)}" : "");
        /// <summary>
        /// Verilen string değer null veya boş (&quot;&quot;) ise, parametre olarak girilen alternatif string değerler arasında ilk dolu olanı döndürür. Eğer hiçbir alternatif değer dolu değilse boş string (&quot;&quot;) döner.
        /// </summary>
        /// <param name="value">Kontrol edilecek ana string değer.</param>
        /// <param name="defaultvalues">Alternatif string değerler listesi.</param>
        /// <returns>
        /// İlk olarak value değeri boş değilse value değerini döner. Aksi halde alternatif değerler arasında bulunan ilk dolu string değeri döner. Eğer hiçbiri dolu değilse boş string (&quot;&quot;) döner.
        /// </returns>
        public static string CoalesceOrDefault(this string value, params string[] defaultvalues)
        {
            value = value.ToStringOrEmpty();
            if (value != "") { return value; }
            return (defaultvalues ?? Array.Empty<string>()).Select(x => x.ToStringOrEmpty()).Where(x => x != "").FirstOrDefault() ?? "";
        }
        /// <summary>
        /// Verilen dize değerinin null veya boş olup olmadığını kontrol eder.
        /// </summary>
        /// <param name="value">Kontrol edilecek dize.</param>
        /// <returns><see langword="true"/>, eğer dize null veya boşsa; aksi takdirde <see langword="false"/>.</returns>
        public static bool IsNullOrEmpty(this string value) => String.IsNullOrEmpty(value.ToStringOrEmpty());
        /// <summary>
        /// Verilen dize değerinin null, boş veya yalnızca beyaz boşluk karakterlerinden (space, tab, newline vb.) oluşup oluşmadığını kontrol eder.
        /// </summary>
        /// <param name="value">Kontrol edilecek dize.</param>
        /// <returns><see langword="true"/>, eğer dize null, boş veya yalnızca beyaz boşluk karakterlerinden oluşuyorsa; aksi takdirde <see langword="false"/>.</returns>
        public static bool IsNullOrWhiteSpace(this string value) => String.IsNullOrWhiteSpace(value.ToStringOrEmpty());
        /// <summary>
        /// Verilen dize değerinin sayısal bir değere dönüştürülüp dönüştürülemeyeceğini kontrol eder.
        /// </summary>
        /// <param name="value">Kontrol edilecek dize.</param>
        /// <param name="numberstyles">Sayının biçimlendirilmesi için kullanılacak sayı stilleri.</param>
        /// <returns><see langword="true"/>, eğer dize bir sayıya dönüştürülebiliyorsa; aksi takdirde <see langword="false"/>.</returns>
        public static bool IsNumeric(this string value, NumberStyles numberstyles = NumberStyles.Integer) => BigInteger.TryParse(value.ToStringOrEmpty(), numberstyles, NumberFormatInfo.InvariantInfo, out _);
        /// <summary>
        /// Belirtilen string değerinin geçerli bir e-Posta adresi olup olmadığını kontrol eder.
        /// </summary>
        /// <param name="value">Kontrol edilecek e-Posta adresi.</param>
        /// <returns>Geçerli bir e-Posta adresi ise <see langword="true"/>, değilse <see langword="false"/> döner.</returns>
        public static bool IsMail(this string value) => _try.TryMailAddress(value, out _);
        /// <summary>
        /// Verilen string&#39;in geçerli bir e-Posta adresi olup olmadığını ve bu adresin host kısmının belirtilen host ile eşleşip eşleşmediğini kontrol eder. Host karşılaştırması büyük/küçük harfe duyarlı değildir ve host parametresi &#39;@&#39; ile başlıyorsa bu karakter yok sayılır.
        /// </summary>
        public static bool IsMailFromHost(this string value, string host)
        {
            host = host.ToStringOrEmpty().ToLower();
            if (host == "") { return false; }
            if (host[0] == '@') { host = host.Substring(1); }
            return _try.TryMailAddress(value, out MailAddress _ma) && _ma.Host == host;
        }
        /// <summary>
        /// Verilen dize değerinin geçerli bir JSON olup olmadığını kontrol eder.
        /// </summary>
        /// <param name="value">Kontrol edilecek dize (JSON).</param>
        /// <param name="jtokentype">Kontrol edilecek JToken türü.</param>
        /// <param name="haschildrentoken">Çocukların kontrol edilip edilmeyeceğini belirten bir değer.</param>
        /// <returns><see langword="true"/>, eğer dize geçerli bir JSON ise; aksi takdirde <see langword="false"/>.</returns>
        public static bool IsJson(this string value, JTokenType jtokentype, bool haschildrentoken)
        {
            var _r = _try.TryJson(value, jtokentype, out JToken _jt);
            if (_r && haschildrentoken) { return _jt.Children().Any(); }
            return _r;
        }
        /// <summary>
        /// Verilen dize değerinin geçerli bir URI olup olmadığını kontrol eder.
        /// </summary>
        /// <param name="value">Kontrol edilecek dize (URI).</param>
        /// <returns><see langword="true"/>, eğer dize geçerli bir URI ise; aksi takdirde <see langword="false"/>.</returns>
        public static bool IsUri(this string value) => _try.TryUri(value, out _);
        /// <summary>
        /// Verilen dizeyi bir nesnenin üyeleri ile biçimlendirir.
        /// </summary>
        /// <typeparam name="TArgument">Biçimlendirilecek nesnenin türü.</typeparam>
        /// <param name="value">Dize.</param>
        /// <param name="argument">Biçimlendirme için kullanılan nesne.</param>
        /// <returns>Biçimlendirilmiş dize.</returns>
        public static string FormatVar<TArgument>(this string value, TArgument argument) where TArgument : class
        {
            HashSet<string> _arm;
            string _p;
            foreach (var pi in typeof(TArgument).GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).ToArray())
            {
                _arm = new HashSet<string>();
                foreach (Match item in Regex.Matches(value, String.Concat(@"\{", pi.Name, @"(\:.*?)?\}")))
                {
                    if (_arm.Contains(item.Value)) { continue; }
                    _arm.Add(item.Value);
                    _p = String.Concat("{0", item.Groups[1].Value, "}");
                    value = value.Replace(item.Value, String.Format(_p, pi.GetValue(argument)));
                }
            }
            return value;
        }
        /// <summary>
        /// Verilen metot ismi ve tip bilgisi kullanılarak bir route ismi oluşturur.
        /// </summary>
        /// <typeparam name="T">Route&#39;un ilişkilendirileceği sınıf tipi (class olmalıdır)</typeparam>
        /// <param name="methodname">Route ile ilişkilendirilecek metot ismi</param>
        /// <param name="usefulltypename">Tam tip ismi (<see cref="Type.FullName"/>) kullanılacak mı? <see langword="false"/> ise kısa tip ismi (<see cref="MemberInfo.Name"/>) kullanılır</param>
        /// <returns>Biçimli route string&#39;i (örn: &quot;/ControllerName/Method&quot; veya &quot;/Namespace.ControllerName/Method&quot;)</returns>
        /// <exception cref="ArgumentException">method parametresi boş veya null olduğunda fırlatılır</exception>
        public static string GetRouteName<T>(this string methodname, bool usefulltypename) where T : class
        {
            Guard.CheckEmpty(methodname, nameof(methodname));
            return $"/{(usefulltypename ? typeof(T).FullName : typeof(T).Name)}/{methodname}";
        }
        /// <summary> Metin içerisindeki tab (\t), satır başı (\r) ve yeni satır (\n) karakterlerini boşluk ile değiştirir ve baştaki ile sondaki gereksiz boşlukları temizler. Null değerlerde güvenli şekilde çalışır.</summary>
        public static string ReplaceTRNSpace(this string value) => value.ToStringOrEmpty().Replace("\t", " ").Replace("\r", " ").Replace("\n", " ").Trim();
        /// <summary> Metin içerisindeki birden fazla ardışık boşluğu tek bir boşluğa indirger ve baştaki ile sondaki gereksiz boşlukları temizler. Null veya boş metinlerde güvenli şekilde çalışır.</summary>
        public static string RemoveMultipleSpace(this string value) => Regex.Replace(value.ToStringOrEmpty(), " +", " ").Trim();
        /// <summary> Belirtilen karakter ile doldurarak bir string değerini belirli bir uzunluğa getirir.</summary>
        /// <param name="value">Uzunluğu ayarlanacak string değeri. </param>
        /// <param name="totalvaluelength">Hedef toplam uzunluk.  Varsayılan değer 2&#39;dir.</param>
        /// <param name="c">Dolgu için kullanılacak karakter.  Varsayılan değer 0&#39;dır.</param>
        /// <param name="direction">Doldurma yönü. &#39;l&#39; sol tarafa (PadLeft), &#39;r&#39; sağ tarafa (PadRight) doldurur.  Varsayılan değer l&#39;dir.</param>
        /// <returns> Belirtilen uzunluğa getirilmiş string değeri. Eğer değer boş ise veya mevcut uzunluk hedef uzunluktan büyük/eşitse orijinal değeri döndürür. </returns>
        /// <exception cref="ArgumentException"><paramref name="totalvaluelength"/> parametresi sıfır veya negatif olduğunda fırlatılır.</exception>
        public static string Replicate(this string value, int totalvaluelength = 2, char c = '0', char direction = 'l')
        {
            value = value.ToStringOrEmpty();
            if (value != "")
            {
                Guard.CheckZeroOrNegative(totalvaluelength, nameof(totalvaluelength));
                if (totalvaluelength <= value.Length) { return value; }
                if (direction == 'l') { return value.PadLeft(totalvaluelength, c); }
                if (direction == 'r') { return value.PadRight(totalvaluelength, c); }
            }
            return "";
        }
        /// <summary>Verilen dizeyi belirtilen uzunluğa kadar keser. </summary>
        /// <param name="value">Kesilecek dize.</param>
        /// <param name="length">Kesim uzunluğu.</param>
        /// <returns>Kesilmiş dize.</returns>
        public static string SubstringUpToLength(this string value, int length)
        {
            Guard.CheckZeroOrNegative(length, nameof(length));
            value = value.ToStringOrEmpty();
            return (value.Length > length ? value.Substring(0, length).Trim() : value);
        }
        /// <summary>Metni belirtilen maksimum uzunluğa kadar kısaltır. Metin belirtilen uzunluğu aşıyorsa sonuna üç nokta (...) ekler. Metin boş veya null ise boş string döner. </summary>
        /// <param name="value">İşlem yapılacak metin</param>
        /// <param name="length">Metnin maksimum uzunluğu</param>
        /// <returns>Kısaltılmış ve gerekiyorsa üç nokta eklenmiş metin</returns>
        public static string SubstringUpToLengthWithEllipsis(this string value, int length)
        {
            value = value.SubstringUpToLength(length);
            if (value == "") { return ""; }
            return String.Concat(value, "...");
        }
        /// <summary>
        /// Bir string&#39;i belirtilen noktalama işaretleri kurallarına göre Başlık Durumuna dönüştürür.
        /// </summary>
        /// <param name="value">Dönüştürülecek string.</param>
        /// <param name="iswhitespace">Boşluk karakterlerinin yeni kelimeleri ayırmak için dikkate alınıp alınmayacağını belirtir.</param>
        /// <param name="punctuations">Kelime ayıran noktalama karakterleri.</param>
        /// <returns>Başlık durumuna dönüştürülmüş string.</returns>
        public static string ToTitleCase(this string value, bool iswhitespace, char[] punctuations)
        {
            value = value.ToStringOrEmpty();
            if (value == "") { return ""; }
            if (value.Length == 1) { return value.ToUpper(); }
            punctuations = (punctuations ?? Array.Empty<char>()).Where(Char.IsPunctuation).ToArray();
            bool _haspunc = punctuations.Length > 0, _newword = true;
            var _sb = new StringBuilder();
            foreach (var item in value.ToCharArray())
            {
                if ((iswhitespace && Char.IsWhiteSpace(item)) || (_haspunc && punctuations.Contains(item)))
                {
                    _sb.Append(item);
                    _newword = true;
                }
                else if (_newword)
                {
                    _sb.Append(Convert.ToString(item).ToUpper());
                    _newword = false;
                }
                else { _sb.Append(Convert.ToString(item).ToLower()); }
            }
            return _sb.ToString();
        }
        /// <summary>
        /// Verilen bir dizeyi, belirtilen türde bir değere dönüştürür. Dönüşüm başarısız olursa, varsayılan değeri döner.
        /// </summary>
        /// <typeparam name="TKey">Dönüşüm yapılacak hedef tür.</typeparam>
        /// <param name="value">Dönüştürülecek dize değeri.</param>
        /// <returns>Dönüştürülen değeri veya dönüşüm başarısızsa varsayılan değeri döner.</returns>
        public static TKey ParseOrDefault<TKey>(this string value)
        {
            var _pd = value.parseordefault_private(typeof(TKey));
            if (_pd.value == null) { return default; }
            try { return (TKey)Convert.ChangeType(_pd.value, _pd.genericbasetype); }
            catch { return default; }
        }
        private static (object value, Type genericbasetype) parseordefault_private(this string value, Type propertytype)
        {
            try
            {
                value = value.ToStringOrEmpty();
                if (value == "") { return (default, default); }
                _ = _try.TryTypeIsNullable(propertytype, out Type _genericbasetype);
                if (_genericbasetype.IsEnum)
                {
                    if (Enum.TryParse(_genericbasetype, value, out object _enum) && Enum.IsDefined(_genericbasetype, _enum)) { return (_enum, _genericbasetype); }
                    return (default, _genericbasetype);
                }
                if (_genericbasetype == typeof(bool))
                {
                    if (value == "0") { return (false, _genericbasetype); }
                    if (value == "1") { return (true, _genericbasetype); }
                    if (Boolean.TryParse(value, out bool _bo)) { return (_bo, _genericbasetype); }
                    return (default, _genericbasetype);
                }
                if (_genericbasetype == typeof(DateOnly))
                {
                    if (DateOnly.TryParse(value, out DateOnly _da)) { return (_da, _genericbasetype); }
                    var _date = value.ParseOrDefault<DateTime?>();
                    if (_date.HasValue) { return (_date.Value.ToDateOnly(), _genericbasetype); }
                    return (default, _genericbasetype);
                }
                if (_genericbasetype == typeof(Uri))
                {
                    if (_try.TryUri(value, out Uri _u)) { return (_u, _genericbasetype); }
                    return (default, _genericbasetype);
                }
                if (_genericbasetype == typeof(MailAddress))
                {
                    if (_try.TryMailAddress(value, out MailAddress _ma)) { return (_ma, _genericbasetype); }
                    return (default, _genericbasetype);
                }
                if (_genericbasetype == typeof(IPAddress))
                {
                    if (IPAddress.TryParse(value, out IPAddress _ip)) { return (_ip, _genericbasetype); }
                    return (default, _genericbasetype);
                }
                if (value.IndexOf('.') > -1 && _genericbasetype.Includes(typeof(float), typeof(double), typeof(decimal))) { value = value.Replace(".", ",", StringComparison.InvariantCulture); }
                return (TypeDescriptor.GetConverter(propertytype).ConvertFrom(value), _genericbasetype);
            }
            catch { return (default, default); }
        }
    }
}