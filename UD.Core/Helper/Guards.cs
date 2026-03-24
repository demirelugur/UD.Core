namespace UD.Core.Helper
{
    using Microsoft.AspNetCore.StaticFiles;
    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using UD.Core.Extensions;
    using UD.Core.Helper.Validation;
    public sealed class Guards
    {
        /// <summary><see cref="CultureInfo.DefaultThreadCurrentUICulture"/>&#39;un iki harfli ISO dil kodunun &quot;en&quot; içerip içermediğini kontrol eder. Bu özellik, uygulamanın geçerli kullanıcı arayüzü kültürünün İngilizce olup olmadığını belirlemek için kullanılabilir. Eğer geçerli UI kültürü İngilizce ise <see langword="true"/> döner, aksi takdirde <see langword="false"/> döner.</summary>
        public static bool IsEnglishDefaultThreadCurrentUICulture => CultureInfo.DefaultThreadCurrentUICulture.TwoLetterISOLanguageName.Equals("en", StringComparison.CurrentCultureIgnoreCase);
        /// <summary>Verilen string&#39;in HTML tag&#39;leri içerip içermediğini kontrol eder. String null, boş veya yalnızca boşluklardan oluşuyorsa <see langword="false"/> döner. HTML tag&#39;leri, düzenli ifade (regex) kullanılarak tespit edilir.</summary>
        /// <param name="value">Kontrol edilecek string.</param>
        /// <returns>String HTML tag&#39;i içeriyorsa <see langword="true"/>, aksi takdirde <see langword="false"/> döner.</returns>
        public static bool IsHtml(string value) => (!value.IsNullOrEmpty() && Regex.IsMatch(value, @"</?\w+\s*[^>]*>", RegexOptions.Compiled));
        /// <summary>Belirtilen path&#39;in tarayıcıda görüntülenebilir bir dosya türüne sahip olup olmadığını kontrol eder. Bu metod, dosya uzantısına göre MIME tipi belirleyerek, tarayıcıların desteklediği türleri tespit eder. PDF dosyaları ve görüntü dosyaları (image/*) tarayıcıda görüntülenebilir olarak kabul edilmez, diğer tüm türler görüntülenebilir olarak değerlendirilir.</summary>
        public static bool IsViewableInBrowser(string path)
        {
            Guard.ThrowIfEmpty(path, nameof(path));
            var uzn = Path.GetExtension(path).ToLower();
            return (uzn == ".pdf" || (new FileExtensionContentTypeProvider().Mappings.TryGetValue(uzn, out string _value) && _value.StartsWith("image/")));
        }
    }
}