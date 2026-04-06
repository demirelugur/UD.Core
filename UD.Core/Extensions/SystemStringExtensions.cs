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
    using UD.Core.Helper.Validation;
    public static class SystemStringExtensions
    {
        /// <summary>Bir string&#39;i Guid&#39;e dönüþtürür. String null veya geçersizse varsayýlan Guid döner.</summary>
        /// <param name="value">Dönüþtürülecek string.</param>
        /// <returns>Dönüþtürülmüþ Guid.</returns>
        public static Guid ToGuid(this string value) => value.ParseOrDefault<Guid>();
        /// <summary>Bir dizeyi <see cref="DateTime"/> türüne dönüþtürür. Dize geçerli bir tarih formatýnda deðilse, varsayýlan <see cref="DateTime"/> deðeri döndürülür.</summary>
        /// <param name="value">Dönüþtürülecek tarih içeren dize.</param>
        /// <returns>Geçerli bir <see cref="DateTime"/> nesnesi veya varsayýlan <see cref="DateTime"/> deðeri.</returns>
        public static DateTime ToDate(this string value) => value.ParseOrDefault<DateTime>();
        private static readonly Dictionary<char, char> charReplacements = new()
        {
            { 'þ', 's' }, { 'Þ', 's' },
            { 'ö', 'o' }, { 'Ö', 'o' },
            { 'ü', 'u' }, { 'Ü', 'u' },
            { 'ç', 'c' }, { 'Ç', 'c' },
            { 'ð', 'g' }, { 'Ð', 'g' },
            { 'ý', 'i' }, { 'I', 'i' }, { 'Ý', 'i' }
        };
        private static readonly char[] charsToRemove = ['?', '/', '.', '\'', '"', '#', '%', '&', '*', '!', '@', '+'];
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
                if (charReplacements.TryGetValue(item, out char _v)) { sb.Append(_v); }
                else if (item == ' ') { sb.Append('-'); }
                else if (Array.IndexOf(charsToRemove, item) == -1) { sb.Append(item); }
            }
            value = sb.ToString().ToLower().Trim();
            value = Regex.Replace(value, @"[^a-z0-9-]", "-");
            value = Regex.Replace(value, @"-+", "-");
            return value.Trim('-');
        }
        /// <summary>
        /// Verilen telefon numarasýný Türk telefon biçime dönüþtürür. Eðer telefon numarasý geçerli bir Türk telefon numarasý deðilse, boþ bir string döner.
        /// <para>Biçim: (###) ###-####</para>
        /// <para>Örneðin: &quot;5001112233&quot; giriþi &quot;(500) 111-2233&quot; biçiminde döner.</para>
        /// </summary>
        /// <param name="phoneNumberTR">Dönüþtürülmek istenen telefon numarasý.</param>
        /// <returns>Biçimlenmiþ Türk telefon numarasý ya da geçerli deðilse boþ bir string.</returns>
        public static string ToPrettyPhoneNumberTR(this string phoneNumberTR) => (TryValidators.TryPhoneNumberTR(phoneNumberTR, out string _s) ? $"({_s.Substring(0, 3)}) {_s.Substring(3, 3)}-{_s.Substring(6, 4)}" : "");
        /// <summary>Verilen string deðer null veya boþ (&quot;&quot;) ise, parametre olarak girilen alternatif string deðerler arasýnda ilk dolu olaný döndürür. Eðer hiçbir alternatif deðer dolu deðilse boþ string (&quot;&quot;) döner.</summary>
        /// <param name="value">Kontrol edilecek ana string deðer.</param>
        /// <param name="defaultValues">Alternatif string deðerler listesi.</param>
        /// <returns>Ýlk olarak value deðeri boþ deðilse value deðerini döner. Aksi halde alternatif deðerler arasýnda bulunan ilk dolu string deðeri döner. Eðer hiçbiri dolu deðilse boþ string (&quot;&quot;) döner.</returns>
        public static string CoalesceOrDefault(this string value, params string[] defaultValues)
        {
            value = value.ToStringOrEmpty();
            if (value == "") { return (defaultValues ?? []).Select(x => x.ToStringOrEmpty()).FirstOrDefault(x => x != "") ?? ""; }
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
            host = host.ToStringOrEmpty().TrimStart('@').ToLower();
            Guard.ThrowIfEmpty(host, nameof(host));
            return TryValidators.TryMailAddress(value, out MailAddress _ma) && _ma.Host == host;
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
        public static string GetRouteName<T>(this string methodName, bool useFullTypeName) where T : class
        {
            Guard.ThrowIfEmpty(methodName, nameof(methodName));
            return $"/{(useFullTypeName ? typeof(T).FullName : typeof(T).Name)}/{methodName}";
        }
        /// <summary>Metin içerisindeki tab (\t), satýr baþý (\r) ve yeni satýr (\n) karakterlerini boþluk ile deðiþtirir ve baþtaki ile sondaki gereksiz boþluklarý temizler. Null deðerlerde güvenli þekilde çalýþýr.</summary>
        public static string ReplaceTRNSpace(this string value) => value.ToStringOrEmpty().Replace("\t", " ").Replace("\r", " ").Replace("\n", " ").Trim();
        /// <summary>Metin içerisindeki birden fazla ardýþýk boþluðu tek bir boþluða indirger ve baþtaki ile sondaki gereksiz boþluklarý temizler. Null veya boþ metinlerde güvenli þekilde çalýþýr.</summary>
        public static string RemoveMultipleSpace(this string value) => Regex.Replace(value.ToStringOrEmpty(), " +", " ").Trim();
        /// <summary>Belirtilen karakter ile doldurarak bir string deðerini belirli bir uzunluða getirir.</summary>
        /// <param name="value">Uzunluðu ayarlanacak string deðeri. </param>
        /// <param name="totalValueLength">Hedef toplam uzunluk.  Varsayýlan deðer 2&#39;dir.</param>
        /// <param name="c">Dolgu için kullanýlacak karakter.  Varsayýlan deðer 0&#39;dýr.</param>
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
            Guard.ThrowIfZeroOrNegative(length, nameof(length));
            value = value.ToStringOrEmpty();
            return (value.Length > length ? value.Substring(0, length).Trim() : value);
        }
        /// <summary>Bir string&#39;i belirtilen noktalama iþaretleri kurallarýna göre Baþlýk Durumuna dönüþtürür.</summary>
        /// <param name="value">Dönüþtürülecek string.</param>
        /// <param name="isWhiteSpace">Boþluk karakterlerinin yeni kelimeleri ayýrmak için dikkate alýnýp alýnmayacaðýný belirtir.</param>
        /// <param name="punctuations">Kelime ayýran noktalama karakterleri.</param>
        /// <returns>Baþlýk durumuna dönüþtürülmüþ string.</returns>
        public static string ToTitleCase(this string value, bool isWhiteSpace, char[] punctuations)
        {
            value = value.ToStringOrEmpty();
            if (value == "") { return ""; }
            if (value.Length == 1) { return value.ToUpper(); }
            punctuations = (punctuations ?? []).Where(Char.IsPunctuation).ToArray();
            bool hasPunctuation = punctuations.Length > 0, newword = true;
            var sb = new StringBuilder();
            foreach (var item in value.ToCharArray())
            {
                if ((isWhiteSpace && Char.IsWhiteSpace(item)) || (hasPunctuation && punctuations.Contains(item)))
                {
                    sb.Append(item);
                    newword = true;
                }
                else if (newword)
                {
                    sb.Append(Convert.ToString(item).ToUpper());
                    newword = false;
                }
                else { sb.Append(Convert.ToString(item).ToLower()); }
            }
            return sb.ToString();
        }
        /// <summary>Verilen metni SQL LIKE sorgusu için &quot;%<paramref name="value"/>%&quot; biçimine getirir <code>.WhereIf(input.Ad.IsNotNullOrEmpty(), x => EF.Functions.Like(x.Ad.ToLower(), input.Ad.LikeContains()))</code></summary>
        public static string LikeContains(this string value, StringCaseHandling caseHandling = StringCaseHandling.lower)
        {
            value = value.ToStringOrEmpty();
            if (value == "") { return ""; }
            return caseHandling switch
            {
                StringCaseHandling.@default => $"%{value}%",
                StringCaseHandling.lower => $"%{value.ToLower()}%",
                StringCaseHandling.upper => $"%{value.ToUpper()}%",
                _ => throw Utilities.ThrowNotSupportedForEnum<StringCaseHandling>()
            };
        }
        /// <summary>Verilen metni SQL LIKE sorgusu için &quot;<paramref name="value"/>%&quot; biçimine getirir <code>.WhereIf(input.Ad.IsNotNullOrEmpty(), x => EF.Functions.Like(x.Ad.ToLower(), input.Ad.LikeStartWith()))</code></summary>
        public static string LikeStartWith(this string value, StringCaseHandling caseHandling = StringCaseHandling.lower)
        {
            value = value.ToStringOrEmpty();
            if (value == "") { return ""; }
            return caseHandling switch
            {
                StringCaseHandling.@default => String.Concat(value, "%"),
                StringCaseHandling.lower => String.Concat(value.ToLower(), "%"),
                StringCaseHandling.upper => String.Concat(value.ToUpper(), "%"),
                _ => throw Utilities.ThrowNotSupportedForEnum<StringCaseHandling>()
            };
        }
        /// <summary>Verilen metni SQL LIKE sorgusu için &quot;%<paramref name="value"/>&quot; biçimine getirir. <code>.WhereIf(input.Ad.IsNotNullOrEmpty(), x => EF.Functions.Like(x.Ad.ToLower(), input.Ad.LikeEndsWith()))</code></summary>
        public static string LikeEndsWith(this string value, StringCaseHandling caseHandling = StringCaseHandling.lower)
        {
            value = value.ToStringOrEmpty();
            if (value == "") { return ""; }
            return caseHandling switch
            {
                StringCaseHandling.@default => String.Concat("%", value),
                StringCaseHandling.lower => String.Concat("%", value.ToLower()),
                StringCaseHandling.upper => String.Concat("%", value.ToUpper()),
                _ => throw Utilities.ThrowNotSupportedForEnum<StringCaseHandling>()
            };
        }
        /// <summary>JSON string&#39;inden belirtilen anahtara (key) karþýlýk gelen deðeri tip güvenli þekilde çeker.</summary>
        /// <typeparam name="T">Döndürülecek deðerin tipi (string, int, bool, DateTime, Guid vb.)</typeparam>
        /// <param name="jsonData">Ýçinden deðer okunacak JSON string&#39;i (JObject olmalýdýr)</param>
        /// <param name="key">Deðeri alýnacak property&#39;nin anahtarý (key)</param>
        /// <returns>Bulunan property deðeri belirtilen türe (T) dönüþtürülerek döndürülür. Property bulunamazsa, null ise veya JSON geçersizse varsayýlan deðer (default(T)) döndürülür.</returns>
        public static T JObjectGetProperty<T>(this string jsonData, string key)
        {
            key = key.ToStringOrEmpty();
            if (key != "" && TryValidators.TryJson(jsonData, JTokenType.Object, out JObject _jo) && _jo.HasValues)
            {
                var k = _jo[key];
                if (k.IsNullOrUndefined()) { return default; }
                return k.ParseOrDefault<T>();
            }
            return default;
        }
        /// <summary>Verilen string ifadesinin SHA-256 hash deðerini hesaplar ve hexadecimal (hex) formatýnda döndürür.</summary>
        /// <param name="value">Hash deðeri hesaplanacak string ifade.</param>
        /// <returns>SHA-256 hash&#39;inin 64 karakterlik hexadecimal string deðeri.</returns>
        /// <remarks>
        /// <para>Bu extension method, string&#39;i önce UTF-8 byte dizisine çevirir ve ardýndan SHA-256 algoritmasý ile hash&#39;ler.</para>
        /// <para>SQL Server karþýlýðý:</para>
        /// <code>SELECT SUBSTRING([sys].[fn_varbintohexstr](HASHBYTES(&#39;SHA2_256&#39;, &#39;Lorem Ipsum&#39;)), 3, 64)</code>
        /// </remarks>
        public static string ToSHA256Hexadecimal(this string value) => Encoding.UTF8.GetBytes(value.ToStringOrEmpty()).ToSHA256Hexadecimal(); // SELECT SUBSTRING([sys].[fn_varbintohexstr](HASHBYTES('SHA2_256', 'Lorem Ipsum')), 3, 64)
        /// <summary>Verilen string ifadesinin SHA-512 hash deðerini hesaplar ve hexadecimal (hex) formatýnda döndürür.</summary>
        /// <param name="value">Hash deðeri hesaplanacak string ifade.</param>
        /// <returns>SHA-512 hash&#39;inin 128 karakterlik hexadecimal string deðeri.</returns>
        /// <remarks>
        /// <para>Bu extension method, string&#39;i önce UTF-8 byte dizisine çevirir ve ardýndan SHA-512 algoritmasý ile hash&#39;ler.</para>
        /// <para>SQL Server karþýlýðý:</para>
        /// <code>SELECT SUBSTRING([sys].[fn_varbintohexstr](HASHBYTES(&#39;SHA2_512&#39;, &#39;Lorem Ipsum&#39;)), 3, 128)</code>
        /// </remarks>
        public static string ToSHA512Hexadecimal(this string value) => Encoding.UTF8.GetBytes(value.ToStringOrEmpty()).ToSHA512Hexadecimal(); // SELECT SUBSTRING([sys].[fn_varbintohexstr](HASHBYTES('SHA2_512', 'Lorem Ipsum')), 3, 128)
    }
}