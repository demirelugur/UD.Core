namespace UD.Core.Helper
{
    using Ganss.Xss;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.StaticFiles;
    using Microsoft.Data.SqlClient;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq.Expressions;
    using System.Security.Cryptography;
    using System.Transactions;
    using UD.Core.Extensions;
    using UD.Core.Helper.Results;
    using UD.Core.Helper.Validation;
    public sealed class Utilities
    {
        /// <summary>
        /// Veritabanı bağlantı dizesi oluşturur. 
        /// <para><c>Data Source=###;Initial Catalog=###;Persist Security Info=True;User ID=###;Password=###;MultipleActiveResultSets=True;TrustServerCertificate=True</c></para>
        /// </summary>
        /// <param name="dataSource">Veritabanı sunucu adı veya IP adresi (DataSource)</param>
        /// <param name="initialCatalog">Bağlanılacak veritabanı adı (Initial Catalog)</param>
        /// <param name="userID">Veritabanı kullanıcı adı (User ID)</param>
        /// <param name="password">Veritabanı kullanıcı şifresi (Password)</param>
        /// <returns>Oluşturulan SQL bağlantı dizesini döndürür</returns>
        /// <remarks>
        /// Oluşturulan bağlantı dizesinde aşağıdaki özellikler ayarlanır:
        /// <list type="bullet">
        /// <item><description><b>PersistSecurityInfo</b>: Güvenlik bilgilerinin bağlantı dizesinde kalması sağlanır</description></item>
        /// <item><description><b>MultipleActiveResultSets</b>: Aynı bağlantı üzerinde birden fazla aktif sonuç kümesine izin verilir (MARS)</description></item>
        /// <item><description><b>TrustServerCertificate</b>: Sunucu sertifikasının doğrulanmadan güvenilir kabul edilmesi sağlanır</description></item>
        /// </list>
        /// </remarks>
        public static string GetConnectionString(string dataSource, string initialCatalog, string userID, string password)
        {
            Guard.ThrowIfEmpty(dataSource, nameof(dataSource));
            Guard.ThrowIfEmpty(initialCatalog, nameof(initialCatalog));
            Guard.ThrowIfEmpty(userID, nameof(userID));
            Guard.ThrowIfEmpty(password, nameof(password));
            return new SqlConnectionStringBuilder
            {
                DataSource = dataSource,
                InitialCatalog = initialCatalog,
                UserID = userID,
                Password = password,
                PersistSecurityInfo = true,
                MultipleActiveResultSets = true,
                TrustServerCertificate = true
            }.ToString();
        }
        /// <summary>Verilen sınıfın belirtilen özelliğindeki maksimum karakter uzunluğunu döner. Eğer <see cref="StringLengthAttribute"/> veya <see cref="MaxLengthAttribute"/> gibi uzunluk sınırlayıcı öznitelikler atanmışsa, bu değeri alır. Aksi takdirde 0 döner.</summary>
        /// <typeparam name="T">Kontrol edilecek sınıf türü.</typeparam>
        /// <param name="name">Kontrol edilecek özelliğin adı.</param>
        /// <returns>Özellik için maksimum uzunluk değeri; öznitelik bulunmazsa 0 döner.</returns>
        public static int GetStringOrMaxLength<T>(string name) where T : class
        {
            Guard.ThrowIfEmpty(name, nameof(name));
            var prop = typeof(T).GetProperty(name);
            Guard.ThrowIfNull(prop, nameof(name));
            return prop.GetStringOrMaxLength();
        }
        /// <summary>Verilen sınıfın belirli bir string ifadesi için maksimum karakter uzunluğunu döner. <see cref="StringLengthAttribute"/> veya <see cref="MaxLengthAttribute"/> atanmışsa bu değeri alır, aksi takdirde 0 döner.</summary>
        /// <typeparam name="T">Kontrol edilecek sınıf türü.</typeparam>
        /// <param name="expression">Özellik ismini içeren ifade.</param>
        /// <returns>Özellik için maksimum uzunluk değeri; öznitelik bulunmazsa 0 döner.</returns>
        public static int GetStringOrMaxLength<T>(Expression<Func<T, string>> expression) where T : class => GetStringOrMaxLength<T>(expression.GetMemberName());
        /// <summary>
        /// Verilen string dizisinden kısaltma oluşturur. Her bir kelimenin baş harfini alarak noktalarla ayrılmış bir kısaltma döner. Boş veya null değerler atlanır.
        /// <example>
        /// <para>Örnek:</para>
        /// <list type="bullet">
        /// <item><description><b>Uğur DEMİREL</b> -&gt; <b>U.D</b></description></item>
        /// <item><description><b>Mustafa Kemal ATATÜRK</b> -&gt; <b>MK.A</b></description></item>
        /// </list>
        /// </example>
        /// </summary>
        /// <param name="names">Kısaltma için kullanılacak isim dizisi.</param>
        /// <returns>Verilen isimlerin baş harflerinden oluşan kısaltma. Eğer parametreler boş veya geçersizse boş string döner.</returns>
        public static string GetNameInitials(params string[] names)
        {
            names = (names ?? []).Select(x => x.ToStringOrEmpty()).Where(x => x != "").ToArray();
            if (names.Length == 0) { return ""; }
            return String.Join(".", names.Select(x => String.Join("", x.ToUpper().Split([' '], StringSplitOptions.RemoveEmptyEntries).Select(x => x[0]).ToArray())));
        }
        /// <summary>Sadece uzantıdan MIME type döner. Eğer herhangi bir kayıt bulunamazsa &quot;application/octet-stream&quot; değerini döner</summary>
        /// <param name="extension">Dosya uzantısı (.txt, .pdf vb.)</param>
        /// <returns>MIME type değeri</returns>
        public static string GetMimeTypeByExtension(string extension)
        {
            extension = extension.ToStringOrEmpty().ToLower();
            if (extension != "")
            {
                if (extension[0] != '.') { extension = String.Concat(".", extension); }
                if (new FileExtensionContentTypeProvider().Mappings.TryGetValue(extension, out string _v)) { return _v; }
            }
            return "application/octet-stream";
        }
        /// <summary>Belirtilen uzunlukta, kriptografik olarak güvenli rastgele bayt dizisi (anahtar) üretir. </summary>
        /// <param name="length">Üretilecek anahtarın bayt cinsinden uzunluğu.</param>
        /// <returns>Rastgele üretilmiş baytlardan oluşan anahtar dizisi.</returns>
        public static byte[] GenerateRandomKey(int length)
        {
            Guard.ThrowIfZeroOrNegative(length, nameof(length));
            using (var rng = RandomNumberGenerator.Create())
            {
                var byteArray = new byte[length];
                rng.GetBytes(byteArray);
                return byteArray;
            }
        }
        /// <summary>Metni belirtilen maksimum uzunluğa kadar kısaltır. Metin belirtilen uzunluğu aşıyorsa sonuna üç nokta (...) ekler. Metin boş veya null ise boş string döner. </summary>
        /// <param name="value">İşlem yapılacak metin</param>
        /// <param name="maxLength">3 nokta dahil metnin maksimum uzunluğu</param>
        /// <returns>Kısaltılmış ve gerekiyorsa üç nokta eklenmiş metin</returns>
        public static string SubstringUpToLengthWithEllipsis(string value, int maxLength)
        {
            value = value.SubstringUpToLength(maxLength - 3);
            if (value == "") { return ""; }
            return String.Concat(value, "...");
        }
        /// <summary>Verilen metindeki Türkçe özel karakterleri (Ç, ç, Ğ, ğ, İ, ı, Ö, ö, Ş, ş, Ü, ü) karşılık gelen İngilizce karakterlerle (C, c, G, g, I, i, O, o, S, s, U, u) değiştirir. Eğer metin null ise, boş bir string döner.</summary>
        /// <param name="value">Türkçe karakterlerin değiştirileceği metin.</param>
        /// <returns>Değiştirilmiş metni döner.</returns>
        public static string ReplaceTurkishChars(string value) => value.ToStringOrEmpty().Replace('Ç', 'C').Replace('ç', 'c').Replace('Ğ', 'G').Replace('ğ', 'g').Replace('İ', 'I').Replace('ı', 'i').Replace('Ö', 'O').Replace('ö', 'o').Replace('Ş', 'S').Replace('ş', 's').Replace('Ü', 'U').Replace('ü', 'u');
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
        /// <param name="entity">Değeri atanac nesne.</param>
        /// <param name="propertyName">Değeri atanacak property&#39;sinin adı.</param>
        /// <param name="propertyNewValue">Property&#39;ye atanacak değer.</param>
        /// <exception cref="ArgumentException"><paramref name="entity"/> nesnesi sınıf türünde değilse fırlatılır.</exception>
        /// <exception cref="InvalidOperationException">Property yazılabilir değilse fırlatılır.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="entity"/> veya <paramref name="propertyName"/> null ise fırlatılır.</exception>
        public static void SetPropertyValue(object entity, string propertyName, object propertyNewValue)
        {
            Guard.ThrowIfNull(entity, nameof(entity));
            Guard.ThrowIfEmpty(propertyName, nameof(propertyName));
            var type = entity.GetType();
            if (!type.IsCustomClass())
            {
                if (Checks.IsEnglishCurrentUICulture) { throw new ArgumentException($"The \"{nameof(entity)}\" argument type must be class!", nameof(entity)); }
                throw new ArgumentException($"\"{nameof(entity)}\" argümanı türü class olmalıdır!", nameof(entity));
            }
            var pi = type.GetProperty(propertyName);
            Guard.ThrowIfNull(pi, nameof(pi));
            if (!pi.CanWrite)
            {
                if (Checks.IsEnglishCurrentUICulture) { throw new InvalidOperationException($"The \"{nameof(propertyName)}\" property is not writable!"); }
                throw new InvalidOperationException($"\"{nameof(propertyName)}\" özelliği yazılabilir değil!");
            }
            pi.SetValue(entity, Converters.ChangeType(propertyNewValue, pi.PropertyType));
        }
        /// <summary>Enum türleri için desteklenmeyen değer hatası oluşturur. Belirtilen Enum türü ve ek detaylarla birlikte bir hata mesajı üretir.</summary>
        /// <typeparam name="TEnum">Enum türü (generic).</typeparam>
        /// <param name="details">Hata mesajına eklenecek isteğe bağlı ek detaylar.</param>
        /// <returns>Desteklenmeyen Enum değerine ait NotSupportedException nesnesi döner.</returns>
        public static NotSupportedException ThrowNotSupportedForEnum<TEnum>(params string[] details) where TEnum : struct, Enum
        {
            var r = new HashSet<string> { typeof(TEnum).FullName, (Checks.IsEnglishCurrentUICulture ? $"The {nameof(Enum)} value is incompatible!" : $"{nameof(Enum)} değeri uyumsuzdur!") };
            if (!details.IsNullOrEmptyOrAllNull()) { r.AddRangeOptimized(details); }
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