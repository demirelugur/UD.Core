namespace UD.Core.Extensions
{
    using Newtonsoft.Json.Linq;
    using System;
    using System.Globalization;
    using System.Net.Mail;
    using System.Numerics;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;
    using UD.Core.Enums;
    using UD.Core.Helper;
    public static partial class SystemStringExtensions
    {
        /// <summary><paramref name="value"/> deðerini <see cref="Guid"/> türüne dönüþtürür. Eðer <paramref name="value"/> geçerli bir Guid biçiminde deðilse, varsayýlan <see cref="Guid"/> deðeri döndürülür.</summary>
        /// <param name="value">Dönüþtürülecek dize.</param>
        /// <param name="format">Kullanýlacak <see cref="EnumGuidFormat"/> formatý.</param>
        /// <returns>Geçerli bir <see cref="Guid"/> nesnesi veya varsayýlan <see cref="Guid"/> deðeri.</returns>
        public static Guid ToGuid(this string value, EnumGuidFormat? format = null)
        {
            value = value.ToStringOrEmpty();
            if (value == "") { return Guid.Empty; }
            return (format.HasValue ? ToGuidFromBase(value, format.Value) : value.ParseOrDefault<Guid>());
        }
        private static Guid ToGuidFromBase(string value, EnumGuidFormat format)
        {
            var (maxLength, chars) = format.GetFormatInfo();
            if (value.Length > maxLength) { throw ToGuidFromBaseFormatException(null); }
            BigInteger number = 0;
            foreach (var character in value)
            {
                var index = chars.IndexOf(character);
                if (index < 0) { throw ToGuidFromBaseFormatException(character); }
                number = (number * chars.Length) + index;
            }
            var bytes = number.ToByteArray(true, true);
            if (bytes.Length > 16) { throw ToGuidFromBaseFormatException(null); }
            var guidBytes = new byte[16];
            Buffer.BlockCopy(bytes, 0, guidBytes, 16 - bytes.Length, bytes.Length);
            return new(guidBytes, true);
        }
        private static FormatException ToGuidFromBaseFormatException(char? c)
        {
            if (c.HasValue)
            {
                if (Checks.IsEnglishCurrentUICulture) { return new($"Invalid character: \"{c.Value}\""); }
                return new($"Geçersiz karakter: \"{c.Value}\"");
            }
            if (Checks.IsEnglishCurrentUICulture) { return new("The value is too large to represent a valid Guid."); }
            return new("Deðer geçerli bir Guid için çok büyük.");
        }
        /// <summary>Bir dizeyi <see cref="DateTime"/> türüne dönüþtürür. Dize geçerli bir tarih biçiminde deðilse, varsayýlan <see cref="DateTime"/> deðeri döndürülür.</summary>
        /// <param name="value">Dönüþtürülecek tarih içeren dize.</param>
        /// <returns>Geçerli bir <see cref="DateTime"/> nesnesi veya varsayýlan <see cref="DateTime"/> deðeri.</returns>
        public static DateTime ToDate(this string value) => value.ParseOrDefault<DateTime>();
        private static readonly Dictionary<char, char> _charReplacements = new()
        {
            { 'þ', 's' }, { 'Þ', 's' },
            { 'ö', 'o' }, { 'Ö', 'o' },
            { 'ü', 'u' }, { 'Ü', 'u' },
            { 'ç', 'c' }, { 'Ç', 'c' },
            { 'ð', 'g' }, { 'Ð', 'g' },
            { 'ý', 'i' }, { 'I', 'i' }, { 'Ý', 'i' }
        };
        private static readonly char[] _charsToRemove = ['?', '/', '.', '\'', '"', '#', '%', '&', '*', '!', '@', '+'];
        /// <summary>Verilen dizeyi SEO dostu bir hale getirir.</summary>
        /// <param name="value">Dönüþtürülecek dize.</param>
        /// <returns>SEO dostu hale getirilmiþ dize.</returns>
        public static string ToSeoFriendly(this string value)
        {
            value = value.ToStringOrEmpty();
            if (value == "") { return ""; }
            var sb = new StringBuilder(value.Length);
            foreach (var item in value.ToCharArray())
            {
                if (_charReplacements.TryGetValue(item, out var _c)) { sb.Append(_c); }
                else if (item == ' ') { sb.Append('-'); }
                else if (Array.IndexOf(_charsToRemove, item) == -1) { sb.Append(item); }
            }
            value = sb.ToString().ToLower().Trim();
            value = RegexPatterns.NonAlphanumericPattern().Replace(value, "-");
            value = RegexPatterns.MultipleHyphensPattern().Replace(value, "-");
            return value.Trim('-');
        }
        /// <summary>
        /// Verilen telefon numarasýný Türk telefon biçime dönüþtürür. Eðer telefon numarasý geçerli bir Türk telefon numarasý deðilse, boþ bir string döner.
        /// <para>Biçim: (###) ###-####</para>
        /// <para>Örneðin: &quot;5001112233&quot; giriþi &quot;(500) 111-2233&quot; biçiminde döner.</para>
        /// </summary>
        /// <param name="phoneNumberTR">Dönüþtürülmek istenen telefon numarasý.</param>
        /// <returns>Biçimlenmiþ Türk telefon numarasý ya da geçerli deðilse boþ bir string.</returns>
        public static string ToPrettyPhoneNumberTR(this string phoneNumberTR) => (TryValidators.TryPhoneNumberTR(phoneNumberTR, out var _s) ? $"({_s.Substring(0, 3)}) {_s.Substring(3, 3)}-{_s.Substring(6, 4)}" : "");
        /// <summary>Verilen string deðer null veya boþ (&quot;&quot;) ise, parametre olarak girilen alternatif string deðerler arasýnda ilk dolu olaný döndürür. Eðer hiçbir alternatif deðer dolu deðilse boþ string (&quot;&quot;) döner.</summary>
        /// <param name="value">Kontrol edilecek ana string deðer.</param>
        /// <param name="defaultValues">Alternatif string deðerler listesi.</param>
        /// <returns>Ýlk olarak value deðeri boþ deðilse value deðerini döner. Aksi halde alternatif deðerler arasýnda bulunan ilk dolu string deðeri döner. Eðer hiçbiri dolu deðilse boþ string (&quot;&quot;) döner.</returns>
        public static string CoalesceOrDefault(this string value, params string[] defaultValues)
        {
            value = value.ToStringOrEmpty();
            if (value == "")
            {
                string s;
                foreach (var item in (defaultValues ?? []))
                {
                    s = item.ToStringOrEmpty();
                    if (s != "") { return s; }
                }
            }
            return value;
        }
        /// <summary>Verilen dize deðerinin null veya boþ olup olmadýðýný kontrol eder.</summary>
        /// <param name="value">Kontrol edilecek dize.</param>
        /// <returns><see langword="true"/>, eðer dize null veya boþsa; aksi takdirde <see langword="false"/>.</returns>
        public static bool IsNullOrEmpty(this string value) => String.IsNullOrEmpty(value.ToStringOrEmpty());
        /// <summary>Verilen dize deðerinin null, boþ veya yalnýzca beyaz boþluk karakterlerinden (space, tab, newline vb.) oluþup oluþmadýðýný kontrol eder.</summary>
        /// <param name="value">Kontrol edilecek dize.</param>
        /// <returns><see langword="true"/>, eðer dize null, boþ veya yalnýzca beyaz boþluk karakterlerinden oluþuyorsa; aksi takdirde <see langword="false"/>.</returns>
        public static bool IsNullOrWhiteSpace(this string value) => String.IsNullOrWhiteSpace(value.ToStringOrEmpty());
        /// <summary>Verilen dize deðerinin sayýsal bir deðere dönüþtürülüp dönüþtürülemeyeceðini kontrol eder.</summary>
        /// <param name="value">Kontrol edilecek dize.</param>
        /// <param name="numberStyles">Sayýnýn biçimlendirilmesi için kullanýlacak sayý stilleri.</param>
        /// <returns><see langword="true"/>, eðer dize bir sayýya dönüþtürülebiliyorsa; aksi takdirde <see langword="false"/>.</returns>
        public static bool IsNumeric(this string value, NumberStyles numberStyles = NumberStyles.Integer) => BigInteger.TryParse(value.ToStringOrEmpty(), numberStyles, NumberFormatInfo.InvariantInfo, out _);
        /// <summary>Belirtilen string deðerinin geçerli bir e-Posta adresi olup olmadýðýný kontrol eder.</summary>
        /// <param name="value">Kontrol edilecek e-Posta adresi.</param>
        /// <returns>Geçerli bir e-Posta adresi ise <see langword="true"/>, deðilse <see langword="false"/> döner.</returns>
        public static bool IsMail(this string value) => TryValidators.TryMailAddress(value, out _);
        /// <summary><paramref name="value"/> deðerinin geçerli bir e-Posta adresi olup olmadýðýný ve e-Posta adresinin host kýsmýnýn <paramref name="host"/> parametresiyle eþleþip eþleþmediðini kontrol eder.</summary>
        /// <param name="value">Kontrol edilecek e-Posta adresi.</param>
        /// <param name="host">Kontrol edilecek host.</param>
        /// <returns><see langword="true"/>, eðer e-Posta adresi geçerli ve host kýsmý belirtilen host ile eþleþiyorsa; aksi takdirde <see langword="false"/>.</returns>
        public static bool IsMailFromHost(this string value, string host)
        {
            host = host.ToStringOrEmpty().TrimStart('@').ToLowerInvariant();
            return TryValidators.TryMailAddress(value, out var _ma) && _ma.Host == host;
        }
        /// <summary>Verilen dize deðerinin geçerli bir URI olup olmadýðýný kontrol eder.</summary>
        /// <param name="value">Kontrol edilecek dize (URI).</param>
        /// <returns><see langword="true"/>, eðer dize geçerli bir URI ise; aksi takdirde <see langword="false"/>.</returns>
        public static bool IsUri(this string value) => TryValidators.TryUri(value, out _);
        /// <summary>Verilen dizeyi bir nesnenin üyeleri ile biçimlendirir.</summary>
        /// <typeparam name="TArgument">Biçimlendirilecek nesnenin türü.</typeparam>
        /// <param name="value">Dize.</param>
        /// <param name="argument">Biçimlendirme için kullanýlan nesne.</param>
        /// <returns>Biçimlendirilmiþ dize.</returns>
        public static string FormatVar<TArgument>(this string value, TArgument argument) where TArgument : class
        {
            HashSet<string> arm;
            string p;
            foreach (var pi in typeof(TArgument).GetProperties())
            {
                arm = [];
                foreach (Match item in Regex.Matches(value, String.Concat(@"\{", pi.Name, @"(\:.*?)?\}")))
                {
                    if (arm.Contains(item.Value)) { continue; }
                    arm.Add(item.Value);
                    p = String.Concat("{0", item.Groups[1].Value, "}");
                    value = value.Replace(item.Value, String.Format(p, pi.GetValue(argument)));
                }
            }
            return value;
        }
        /// <summary>Verilen metot ismi ve tip bilgisi kullanýlarak bir route ismi oluþturur.</summary>
        /// <typeparam name="T">Route&#39;un iliþkilendirileceði sýnýf tipi (class olmalýdýr)</typeparam>
        /// <param name="methodName">Route ile iliþkilendirilecek metot ismi</param>
        /// <param name="useFullTypeName">Tam tip ismi (<see cref="Type.FullName"/>) kullanýlacak mý? <see langword="false"/> ise kýsa tip ismi (<see cref="MemberInfo.Name"/>) kullanýlýr</param>
        /// <returns>Biçimli route string&#39;i (örn: &quot;/ControllerName/Method&quot; veya &quot;/Namespace.ControllerName/Method&quot;)</returns>
        /// <exception cref="ArgumentException">method parametresi boþ veya null olduðunda fýrlatýlýr</exception>
        public static string GetRouteName<T>(this string methodName, bool useFullTypeName) where T : class => $"/{(useFullTypeName ? typeof(T).FullName : typeof(T).Name)}/{methodName}";
        /// <summary>Metin içerisindeki tab (\t), satýr baþý (\r) ve yeni satýr (\n) karakterlerini boþluk ile deðiþtirir ve baþtaki ile sondaki gereksiz boþluklarý temizler. Null deðerlerde güvenli þekilde çalýþýr.</summary>
        public static string ReplaceTRNSpace(this string value) => value.ToStringOrEmpty().Replace('\t', ' ').Replace('\r', ' ').Replace('\n', ' ').Trim();
        /// <summary>Metin içerisindeki birden fazla ardýþýk boþluðu tek bir boþluða indirger ve baþtaki ile sondaki gereksiz boþluklarý temizler. Null veya boþ metinlerde güvenli þekilde çalýþýr.</summary>
        public static string RemoveMultipleSpace(this string value) => RegexPatterns.MultipleSpacesPattern().Replace(value.ToStringOrEmpty(), " ").Trim();
        /// <summary>Belirtilen karakter ile doldurarak bir string deðerini belirli bir uzunluða getirir.</summary>
        /// <param name="value">Uzunluðu ayarlanacak string deðeri.</param>
        /// <param name="totalValueLength">Hedef toplam uzunluk. Varsayýlan deðer 2&#39;dir.</param>
        /// <param name="c">Dolgu için kullanýlacak karakter. Varsayýlan deðer 0&#39;dýr.</param>
        /// <param name="fillingDirectionIsLeft">Dolgu karakterinin eklenme yönü. <see langword="true"/> ise sol tarafa, <see langword="false"/> ise sað tarafa eklenir. Varsayýlan deðer <see langword="true"/> (sol tarafa doldurma)&#39;dýr.</param>
        /// <returns>Belirtilen uzunluða getirilmiþ string deðeri. Eðer deðer boþ ise veya mevcut uzunluk hedef uzunluktan büyük/eþitse orijinal deðeri döndürür. </returns>
        /// <exception cref="ArgumentException"><paramref name="totalValueLength"/> parametresi sýfýr veya negatif olduðunda fýrlatýlýr.</exception>
        public static string Replicate(this string value, int totalValueLength = 2, char c = '0', bool fillingDirectionIsLeft = true)
        {
            value = value.ToStringOrEmpty();
            if (value == "") { return ""; }
            if (totalValueLength <= value.Length) { return value; }
            return (fillingDirectionIsLeft ? value.PadLeft(totalValueLength, c) : value.PadRight(totalValueLength, c));
        }
        /// <summary>Verilen dizeyi belirtilen uzunluða kadar keser. </summary>
        /// <param name="value">Kesilecek dize.</param>
        /// <param name="length">Kesim uzunluðu.</param>
        /// <returns>Kesilmiþ dize.</returns>
        public static string SubstringUpToLength(this string value, int length)
        {
            value = value.ToStringOrEmpty();
            return (value.Length > length ? value.Substring(0, length).Trim() : value);
        }
        private static readonly string[] _lowerCaseWords = ["Ancak", "Ama", "Da", "De", "Fakat", "Gibi", "Ýle", "Ýse", "Ki", "Lakin", "Ve", "Veya"];
        /// <summary><paramref name="value"/> deðerini baþlýk biçimine (Title Case) dönüþtürür. Her kelimenin ilk harfi büyük, geri kalan harfler küçük olur. Küçük harfe çevrilecek baðlaçlar: <c>Ancak,Ama,Da,De,Fakat,Gibi,Ýle,Ýse,Ki,Lakin,Ve,Veya</c></summary>
        /// <param name="value">Dönüþtürülecek string.</param>
        /// <returns>Baþlýk durumuna dönüþtürülmüþ string.</returns>
        public static string ToTitleCase(this string value)
        {
            value = value.ToTitleCase(true, ['.', '+', '(', '-']);
            if (value == "") { return ""; }
            var cultureInfo = new CultureInfo("tr-TR");
            foreach (var word in _lowerCaseWords) { value = value.Replace($" {word} ", $" {word.ToLower(cultureInfo)} "); }
            return value;
        }
        /// <summary><paramref name="value"/> deðerini baþlýk biçimine (Title Case) dönüþtürür. Her kelimenin ilk harfi büyük, geri kalan harfler küçük olur. Kelimeler arasýndaki ayracý belirlemek için <paramref name="isWhiteSpace"/> ve <paramref name="punctuations"/> parametreleri kullanýlýr. <paramref name="cultureInfo"/> parametresi ile kültüre özgü büyük/küçük harf dönüþümü saðlanabilir (varsayýlan olarak Türkçe kültürü kullanýlýr).</summary>
        /// <param name="value">Dönüþtürülecek string.</param>
        /// <param name="isWhiteSpace">Boþluk karakterlerinin yeni kelimeleri ayýrmak için dikkate alýnýp alýnmayacaðýný belirtir.</param>
        /// <param name="punctuations">Kelime ayýran noktalama karakterleri.</param>
        /// <param name="cultureInfo">Kültür bilgisi. Eðer null ise varsayýlan olarak new CultureInfo(&quot;tr-TR&quot;) kullanýlýr.</param>
        /// <returns>Baþlýk durumuna dönüþtürülmüþ string.</returns>
        public static string ToTitleCase(this string value, bool isWhiteSpace, char[] punctuations, CultureInfo cultureInfo = null)
        {
            value = value.ToStringOrEmpty();
            if (value == "") { return value; }
            var separators = new HashSet<char>(punctuations ?? []);
            if (isWhiteSpace) { separators.Add(' '); }
            cultureInfo ??= CultureInfo.GetCultureInfo("tr-TR");
            var sb = new StringBuilder(value.Length);
            var newWord = true;
            foreach (var ch in value)
            {
                if (separators.Contains(ch))
                {
                    sb.Append(ch);
                    newWord = true;
                }
                else if (newWord)
                {
                    sb.Append(Char.ToUpper(ch, cultureInfo));
                    newWord = false;
                }
                else { sb.Append(Char.ToLower(ch, cultureInfo)); }
            }
            return sb.ToString();
        }
        /// <summary>JSON string&#39;inden belirtilen anahtara (key) karþýlýk gelen deðeri tip güvenli þekilde çeker.</summary>
        /// <typeparam name="T">Döndürülecek deðerin tipi (string, int, bool, DateTime, Guid vb.)</typeparam>
        /// <param name="json">Ýçinden deðer okunacak JSON string&#39;i (JObject olmalýdýr)</param>
        /// <param name="key">Deðeri alýnacak property&#39;nin anahtarý (key)</param>
        /// <returns>Bulunan property deðeri belirtilen türe (T) dönüþtürülerek döndürülür. Property bulunamazsa, null ise veya JSON geçersizse varsayýlan deðer (default(T)) döndürülür.</returns>
        public static T GetPropertyValueFromJObject<T>(this string json, string key)
        {
            key = key.ToStringOrEmpty();
            if (key != "" && TryValidators.TryJson(json, JTokenType.Object, out JObject _jo) && _jo.HasValues)
            {
                var jToken = _jo[key];
                if (jToken.IsNoneOrNullOrUndefined()) { return default; }
                return jToken.ParseOrDefault<T>();
            }
            return default;
        }
        /// <summary>Verilen metnin SHA-256 veya SHA-512 hash deðerini hesaplayarak hexadecimal (hex) biçiminde döndürür.</summary>
        /// <param name="value">Hash deðeri hesaplanacak metin.</param>
        /// <param name="is512"><see langword="true"/> ise SHA-512, <see langword="false"/> ise SHA-256 algoritmasý kullanýlýr.</param>
        /// <returns>SHA-512 için 128 karakter, SHA-256 için 64 karakter uzunluðunda hexadecimal hash deðeri.</returns>
        /// <remarks>
        /// <para>Metin önce UTF-8 byte dizisine dönüþtürülür, ardýndan seçilen SHA algoritmasý ile hash deðeri hesaplanýr.</para>
        /// <para>SQL Server karþýlýklarý:</para>
        /// <code>
        /// -- SHA-512
        /// SELECT SUBSTRING(sys.fn_varbintohexstr(HASHBYTES(&#39;SHA2_512&#39;, &#39;Lorem Ipsum&#39;)), 3, 128)
        /// -- SHA-256
        /// SELECT SUBSTRING(sys.fn_varbintohexstr(HASHBYTES(&#39;SHA2_256&#39;, &#39;Lorem Ipsum&#39;)), 3, 64)
        /// </code>
        /// </remarks>s
        public static string ComputeHash(this string value, bool is512) => Encoding.UTF8.GetBytes(value.ToStringOrEmpty()).ComputeHash(is512); // SHA2_512 ->  SELECT SUBSTRING([sys].[fn_varbintohexstr](HASHBYTES('SHA2_512', 'Lorem Ipsum')), 3, 128), SHA2_256 ->  SELECT SUBSTRING([sys].[fn_varbintohexstr](HASHBYTES('SHA2_256', 'Lorem Ipsum')), 3, 64)
    }
}