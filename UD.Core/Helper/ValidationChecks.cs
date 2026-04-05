namespace UD.Core.Helper
{
    using Microsoft.AspNetCore.StaticFiles;
    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using UD.Core.Extensions;
    using UD.Core.Helper.Validation;
    public sealed class ValidationChecks
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
    }
}