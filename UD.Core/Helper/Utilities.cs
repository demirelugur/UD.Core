namespace UD.Core.Helper
{
    using Ganss.Xss;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Transactions;
    using UD.Core.Extensions;
    using UD.Core.Helper.Validation;
    public sealed class Utilities
    {
        /// <summary>Asenkron işlemler için TransactionScope oluşturur. TransactionScope, işlem bütünlüğünü sağlamak için kullanılır. Bu metod, asenkron işlemlerin TransactionScope ile birlikte kullanılabilmesi için ayarlanmıştır. <code>new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled);</code></summary>
        public static TransactionScope TransactionScopeAsync => new(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled);
        /// <summary>Default HttpContext nesnesi oluşturur. Bu nesne, ASP.NET Core uygulamalarında HTTP isteklerini ve yanıtlarını temsil eder. Bu özellik, testler veya diğer durumlarda gerçek bir HTTP bağlamına ihtiyaç duyulduğunda kullanılabilir. Oluşturulan HttpContext nesnesi, MVC Core ve Server-Side Blazor hizmetlerini içeren bir servis sağlayıcıya sahiptir, böylece bu hizmetlere erişim sağlanabilir.</summary>
        public static DefaultHttpContext GetDefaultHttpContext
        {
            get
            {
                var service = new ServiceCollection();
                service.AddLogging();
                service.AddMvcCore();
                service.AddServerSideBlazor();
                return new DefaultHttpContext
                {
                    RequestServices = service.BuildServiceProvider(),
                    RequestAborted = CancellationToken.None
                };
            }
        }
        /// <summary>Verilen metni Sezar şifreleme algoritması ile şifreler. Belirtilen anahtar (key) değeri kadar harfler kaydırılarak şifreleme yapılır.</summary>
        /// <param name="value">Şifrelenecek metin</param>
        /// <param name="key">Harflerin kaydırılacağı değer</param>
        /// <returns>Şifrelenmiş metin</returns>
        public static string CaesarCipherOperation(string value, int key)
        {
            if (key < 0) { return CaesarCipherOperation(value, key + 26); }
            var r = "";
            foreach (var item in value.ToStringOrEmpty().ToCharArray())
            {
                if ((item >= 'A' && item <= 'Z')) { r = String.Concat(r, Convert.ToChar(((item - 'A' + key) % 26) + 'A').ToString()); }
                else if ((item >= 'a' && item <= 'z')) { r = String.Concat(r, Convert.ToChar(((item - 'a' + key) % 26) + 'a').ToString()); }
                else { r = String.Concat(r, item.ToString()); }
            }
            return r;
        }
        /// <summary>Belirtilen nesnenin property adını kullanarak ilgili property&#39;sine değer atar.</summary>
        /// <param name="value">Değeri atanac nesne.</param>
        /// <param name="propertyName">Değeri atanacak property&#39;sinin adı.</param>
        /// <param name="data">Property&#39;ye atanacak değer.</param>
        /// <exception cref="ArgumentException"><paramref name="value"/> nesnesi sınıf türünde değilse fırlatılır.</exception>
        /// <exception cref="InvalidOperationException">Property yazılabilir değilse fırlatılır.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> veya <paramref name="propertyName"/> null ise fırlatılır.</exception>
        public static void SetPropertyValue(object value, string propertyName, object data)
        {
            Guard.ThrowIfNull(value, nameof(value));
            Guard.ThrowIfEmpty(propertyName, nameof(propertyName));
            var type = value.GetType();
            if (!type.IsCustomClass())
            {
                if (ValidationChecks.IsEnglishDefaultThreadCurrentUICulture) { throw new ArgumentException($"The \"{nameof(value)}\" argument type must be class!", nameof(value)); }
                throw new ArgumentException($"\"{nameof(value)}\" argümanı türü class olmalıdır!", nameof(value));
            }
            var pi = type.GetProperty(propertyName);
            Guard.ThrowIfNull(pi, nameof(pi));
            if (!pi.CanWrite)
            {
                if (ValidationChecks.IsEnglishDefaultThreadCurrentUICulture) { throw new InvalidOperationException($"The \"{nameof(propertyName)}\" property is not writable!"); }
                throw new InvalidOperationException($"\"{nameof(propertyName)}\" özelliği yazılabilir değil!");
            }
            pi.SetValue(value, Converters.ChangeType(data, pi.PropertyType));
        }
        /// <summary><paramref name="culture"/> parametresi ile belirtilen kültür bilgisini kullanarak, uygulamanın varsayılan thread kültürünü ve varsayılan thread UI kültürünü ayarlar. Bu metod, uygulamanın farklı kültürlerde çalışmasını sağlamak için kullanılabilir. <paramref name="culture"/> geçerli bir kültür kodu (örneğin &quot;en-US&quot;, &quot;tr-TR&quot;) içermelidir. Eğer geçerli bir kültür kodu sağlanmazsa, bir hata fırlatılır.</summary>
        public static void SetDefaultThreadCulture(string culture)
        {
            Guard.ThrowIfEmpty(culture, nameof(culture));
            var ci = new CultureInfo(culture);
            Guard.ThrowIfNull(ci, nameof(ci));
            CultureInfo.DefaultThreadCurrentCulture = ci;
            CultureInfo.DefaultThreadCurrentUICulture = ci;
        }
        /// <summary>Enum türleri için desteklenmeyen değer hatası oluşturur. Belirtilen Enum türü ve ek detaylarla birlikte bir hata mesajı üretir.</summary>
        /// <typeparam name="TEnum">Enum türü (generic).</typeparam>
        /// <param name="details">Hata mesajına eklenecek isteğe bağlı ek detaylar.</param>
        /// <returns>Desteklenmeyen Enum değerine ait NotSupportedException nesnesi döner.</returns>
        public static NotSupportedException ThrowNotSupportedForEnum<TEnum>(params string[] details) where TEnum : struct, Enum
        {
            var r = new HashSet<string> { typeof(TEnum).FullName, (ValidationChecks.IsEnglishDefaultThreadCurrentUICulture ? $"The {nameof(Enum)} value is incompatible!" : $"{nameof(Enum)} değeri uyumsuzdur!") };
            if (!details.IsNullOrCountZero()) { r.AddRangeOptimized(details); }
            return new(String.Join(" ", r));
        }
        /// <summary>Script etiketlerini varsayılan olarak temizleyen bir HtmlSanitizer nesnesi oluşturur. Bu metod, HTML içeriğini temizlemek ve güvenli hale getirmek için kullanılabilir. Oluşturulan HtmlSanitizer nesnesi, script etiketlerini temizleyerek potansiyel XSS saldırılarına karşı koruma sağlar. İsteğe bağlı olarak, farklı temizleme seçenekleri belirten bir HtmlSanitizerOptions nesnesi de sağlanabilir.</summary>
        public static HtmlSanitizer CreateSanitizer(HtmlSanitizerOptions? options = null)
        {
            var hs = new HtmlSanitizer(options ?? new()); // XSS örnekleri: https://github.com/pgaijin66/XSS-Payloads/blob/master/payload/payload.txt
            hs.AllowedTags.Remove("script");
            hs.AllowedAttributes.Remove("onerror");
            hs.AllowedAttributes.Remove("onclick");
            return hs;
        }
    }
}