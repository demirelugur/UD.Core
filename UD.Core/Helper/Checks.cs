namespace UD.Core.Helper
{
    using Microsoft.AspNetCore.StaticFiles;
    using System;
    using System.Globalization;
    using System.Numerics;
    using System.Text.RegularExpressions;
    using UD.Core.Extensions;
    using UD.Core.Helper.Validations;
    using static UD.Core.Helper.GlobalConstants;
    public sealed class Checks
    {
        /// <summary><see cref="CultureInfo.CurrentUICulture"/>&#39;un iki harfli ISO dil kodunun &quot;en&quot; içerip içermediğini kontrol eder. Bu özellik, uygulamanın geçerli kullanıcı arayüzü kültürünün İngilizce olup olmadığını belirlemek için kullanılabilir. Eğer geçerli UI kültürü İngilizce ise <see langword="true"/> döner, aksi takdirde <see langword="false"/> döner.</summary>
        public static bool IsEnglishCurrentUICulture => CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.Equals("en", StringComparison.CurrentCultureIgnoreCase);
        /// <summary><paramref name="value"/> değerinin HTML içeriği içerip içermediğini kontrol eder. Bu metod, verilen string değerin HTML etiketleri içerip içermediğini belirlemek için düzenli ifadeler kullanır. Eğer string değerde HTML etiketleri bulunursa, bu metod <see langword="true"/> döner; aksi takdirde <see langword="false"/> döner. Bu kontrol, kullanıcı tarafından sağlanan verilerin HTML içeriği içerip içermediğini tespit etmek ve potansiyel XSS saldırılarına karşı önlem almak için kullanılabilir.</summary>
        public static bool IsHtml(string value) => Regex.IsMatch(value.ToStringOrEmpty(), @"</?\w+\s*[^>]*>", RegexOptions.Compiled);
        /// <summary>Belirtilen path&#39;in tarayıcıda görüntülenebilir bir dosya türüne sahip olup olmadığını kontrol eder. Bu metod, dosya uzantısına göre MIME tipi belirleyerek, tarayıcıların desteklediği türleri tespit eder. PDF dosyaları ve görüntü dosyaları (image/*) tarayıcıda görüntülenebilir olarak kabul edilmez, diğer tüm türler görüntülenebilir olarak değerlendirilir.</summary>
        public static bool IsViewableInBrowser(string path)
        {
            Guard.ThrowIfEmpty(path, nameof(path));
            var uzn = Path.GetExtension(path).ToLower();
            return (uzn == ".pdf" || (new FileExtensionContentTypeProvider().Mappings.TryGetValue(uzn, out string _value) && _value.StartsWith("image/")));
        }
        /// <summary><paramref name="iban"/> değerinin geçerli bir <see cref="TitleConstants.Iban"/> biçimine sahip olup olmadığını kontrol eder. Bu metod, IBAN numarasının uzunluğunu, karakterlerini ve doğrulama algoritmasını kullanarak geçerliliğini değerlendirir. IBAN numarası, ülke kodu, kontrol basamakları ve banka hesap numarası gibi bileşenlerden oluşur. Eğer verilen IBAN numarası geçerli ise <see langword="true"/> döner; aksi takdirde <see langword="false"/> döner. Bu kontrol, finansal işlemlerde doğru ve geçerli IBAN numaralarının kullanılmasını sağlamak için önemlidir.</summary>
        public static bool IsIBANValid(string iban)
        {
            iban = iban.ToStringOrEmpty().Replace(" ", "").ToUpperInvariant();
            if (iban.Length < 15 || iban.Length > 34 || iban.Any(x => !Char.IsLetterOrDigit(x))) { return false; }
            var rearranged = String.Concat(iban[4..], iban[..4]);
            var numericIban = String.Concat(rearranged.Select(x => Char.IsDigit(x) ? x.ToString() : (x - 'A' + 10).ToString()));
            return BigInteger.TryParse(numericIban, out BigInteger _bi) && _bi % 97 == 1;
        }
        /// <summary><paramref name="value"/> değerinin güçlü bir şifre olup olmadığını kontrol eder. Bu metod, şifrenin minimum uzunlukta olup olmadığını ve en az bir rakam, bir küçük harf, bir büyük harf ve bir özel karakter içerip içermediğini değerlendirir. Eğer şifre bu kriterleri karşılıyorsa <see langword="true"/> döner; aksi takdirde <see langword="false"/> döner. Bu kontrol, kullanıcıların güvenli şifreler oluşturmasını sağlamak için kullanılabilir.</summary>
        /// <param name="value">Kontrol edilecek şifre değeri.</param>
        /// <param name="minimumLength">Şifrenin minimum uzunluğu.</param>
        /// <returns><see langword="true"/> şifre güçlü ise, aksi takdirde <see langword="false"/>.</returns>
        public static bool IsStrongPassword(string value, int minimumLength = 8)
        {
            value = value.ToStringOrEmpty();
            var r = value.Length >= minimumLength;
            if (r) { r = Regex.IsMatch(value, @"[\d]"); }
            if (r) { r = Regex.IsMatch(value, @"[a-z]"); }
            if (r) { r = Regex.IsMatch(value, @"[A-Z]"); }
            if (r) { r = Regex.IsMatch(value, @"[!@#$%^&*()_+\-=\[\]{}|;:',.<>?]"); }
            return r;
        }
    }
}