namespace UD.Core.Extensions
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Net;
    using System.Net.Mail;
    using System.Numerics;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;
    using UD.Core.Enums;
    using UD.Core.Helper.Validation;
    using static UD.Core.Helper.OrtakTools;
    public static class StringExtensions
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
        public static string BeautifyPhoneNumberTR(this string phoneNumberTR) => (Validators.TryPhoneNumberTR(phoneNumberTR, out string _s) ? $"({_s.Substring(0, 3)}) {_s.Substring(3, 3)}-{_s.Substring(6, 4)}" : "");
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
        public static bool IsMail(this string value) => Validators.TryMailAddress(value, out _);
        /// <summary>Verilen string&#39;in geçerli bir e-Posta adresi olup olmadýðýný ve bu adresin host kýsmýnýn belirtilen host ile eþleþip eþleþmediðini kontrol eder. Host karþýlaþtýrmasý büyük/küçük harfe duyarlý deðildir ve host parametresi &#39;@&#39; ile baþlýyorsa bu karakter yok sayýlýr.</summary>
        public static bool IsMailFromHost(this string value, string host)
        {
            host = host.ToStringOrEmpty().TrimStart('@').ToLower();
            if (host == "") { return false; }
            return Validators.TryMailAddress(value, out MailAddress _ma) && _ma.Host == host;
        }
        /// <summary>Verilen dize deðerinin geçerli bir URI olup olmadýðýný kontrol eder.</summary>
        /// <param name="value">Kontrol edilecek dize (URI).</param>
        /// <returns><see langword="true"/>, eðer dize geçerli bir URI ise; aksi takdirde <see langword="false"/>.</returns>
        public static bool IsUri(this string value) => Validators.TryUri(value, out _);
        /// <summary>Verilen dizeyi bir nesnenin üyeleri ile biçimlendirir.</summary>
        /// <typeparam name="TArgument">Biçimlendirilecek nesnenin türü.</typeparam>
        /// <param name="value">Dize.</param>
        /// <param name="argument">Biçimlendirme için kullanýlan nesne.</param>
        /// <returns>Biçimlendirilmiþ dize.</returns>
        public static string FormatVar<TArgument>(this string value, TArgument argument) where TArgument : class
        {
            HashSet<string> arm;
            string p;
            foreach (var pi in typeof(TArgument).GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).ToArray())
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
        /// <summary>Verilen bir dizeyi, belirtilen türde bir deðere dönüþtürür. Dönüþüm baþarýsýz olursa, varsayýlan deðeri döner.</summary>
        /// <typeparam name="TKey">Dönüþüm yapýlacak hedef tür.</typeparam>
        /// <param name="value">Dönüþtürülecek dize deðeri.</param>
        /// <returns>Dönüþtürülen deðeri veya dönüþüm baþarýsýzsa varsayýlan deðeri döner.</returns>
        public static TKey ParseOrDefault<TKey>(this string value)
        {
            var pd = parseOrDefault(value, typeof(TKey));
            if (pd.value == null) { return default; }
            try { return (TKey)Convert.ChangeType(pd.value, pd.genericBaseType); }
            catch { return default; }
        }
        private static (object value, Type genericBaseType) parseOrDefault(string value, Type propertyType)
        {
            try
            {
                value = value.ToStringOrEmpty();
                if (value == "") { return (default, default); }
                _ = Validators.TryTypeIsNullable(propertyType, out Type _genericBaseType);
                if (_genericBaseType.IsEnum)
                {
                    if (Enum.TryParse(_genericBaseType, value, out object _enum) && Enum.IsDefined(_genericBaseType, _enum)) { return (_enum, _genericBaseType); }
                    return (default, _genericBaseType);
                }
                if (_genericBaseType == typeof(bool))
                {
                    if (value == "0") { return (false, _genericBaseType); }
                    if (value == "1") { return (true, _genericBaseType); }
                    if (Boolean.TryParse(value, out bool _bo)) { return (_bo, _genericBaseType); }
                    return (default, _genericBaseType);
                }
                if (_genericBaseType == typeof(DateOnly))
                {
                    if (DateOnly.TryParse(value, out DateOnly _da)) { return (_da, _genericBaseType); }
                    var date = value.ParseOrDefault<DateTime?>();
                    if (date.HasValue) { return (date.Value.ToDateOnly(), _genericBaseType); }
                    return (default, _genericBaseType);
                }
                if (_genericBaseType == typeof(Uri))
                {
                    if (Validators.TryUri(value, out Uri _u)) { return (_u, _genericBaseType); }
                    return (default, _genericBaseType);
                }
                if (_genericBaseType == typeof(MailAddress))
                {
                    if (Validators.TryMailAddress(value, out MailAddress _ma)) { return (_ma, _genericBaseType); }
                    return (default, _genericBaseType);
                }
                if (_genericBaseType == typeof(IPAddress))
                {
                    if (IPAddress.TryParse(value, out IPAddress _ip)) { return (_ip, _genericBaseType); }
                    return (default, _genericBaseType);
                }
                if (value.IndexOf('.') > -1 && _genericBaseType.Includes(typeof(float), typeof(double), typeof(decimal))) { value = value.Replace(".", ",", StringComparison.InvariantCulture); }
                return (TypeDescriptor.GetConverter(propertyType).ConvertFrom(value), _genericBaseType);
            }
            catch { return (default, default); }
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
    }
}