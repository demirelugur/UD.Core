namespace UD.Core.Extensions
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Primitives;
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Web;
    using UD.Core.Helper;
    using UD.Core.Helper.Validation;
    using static UD.Core.Enums.BaseEnumResponseMessage;
    public static class AspNetCoreExtensions
    {
        #region HttpContext
        /// <summary>İstemcinin mobil bir cihaz olup olmadığını kontrol eder. </summary>
        /// <param name="context">HttpContext nesnesi.</param>
        /// <returns>Mobil bir cihaz ise <see langword="true"/>, değilse <see langword="false"/> döner.</returns>
        public static bool IsMobileDevice(this HttpContext context)
        {
            Guard.ThrowIfNull(context, nameof(context));
            var userAgent = context.Request.Headers.UserAgent.ToStringOrEmpty().ToLower();
            if (userAgent != "") { foreach (var item in new string[] { "android", "iphone", "ipad", "mobile" }) { if (userAgent.Contains(item)) { return true; } } }
            return false;
        }
        /// <summary>Mevcut HTTP isteğinin şema (http/https) ve host bilgisini kullanarak uygulamanın temel (base) adresini Uri olarak döner. </summary>
        public static Uri GetBaseUri(this HttpContext context)
        {
            Guard.ThrowIfNull(context, nameof(context));
            var request = context.Request;
            return new($"{request.Scheme}://{(request.Host.HasValue ? request.Host.Value : "")}");
        }
        /// <summary>Mevcut HTTP isteğinin tam adresini (base adres + path + query string) Uri formatında döner. </summary>
        public static Uri GetFullRequestUri(this HttpContext context)
        {
            Guard.ThrowIfNull(context, nameof(context));
            var request = context.Request;
            return new(String.Concat(context.GetBaseUri().ToString().TrimEnd('/'), request.Path.HasValue ? request.Path.Value : "", request.QueryString.HasValue ? request.QueryString.Value : ""));
        }
        /// <summary>Bearer token&#39;ı HttpContext&#39;den alır.</summary>
        /// <param name="context">HttpContext nesnesi.</param>
        /// <returns>Bearer token.</returns>
        public static string GetToken(this HttpContext context)
        {
            Guard.ThrowIfNull(context, nameof(context));
            return context.Request.Headers.Authorization.ToString().Replace("Bearer ", "").ToStringOrEmpty();
        }
        /// <summary>İstemcinin IP adresini döndürür. Öncelikle <c>X-Forwarded-For</c> HTTP başlığını kontrol eder; eğer geçerli bir IP bulunamazsa bağlantının <see cref="ConnectionInfo.RemoteIpAddress"/> değerini kullanır. Geçerli bir IP adresi elde edilemezse <see cref="IPAddress.Any"/> döndürülür.</summary>
        /// <param name="context">HTTP isteğini temsil eden <see cref="HttpContext"/> nesnesi.</param>
        /// <returns>İstemcinin IPv4 formatındaki IP adresi veya bulunamazsa <see cref="IPAddress.Any"/>.</returns>
        public static IPAddress GetIPAddress(this HttpContext context)
        {
            Guard.ThrowIfNull(context, nameof(context));
            if (IPAddress.TryParse(context.Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? "", out IPAddress _ip)) { return _ip.MapToIPv4(); }
            _ip = context.Connection.RemoteIpAddress;
            return (_ip == null ? IPAddress.Any : _ip.MapToIPv4());
        }
        #endregion
        #region IFormCollection
        /// <summary>Belirtilen anahtar ile form verilerinden bir değeri alır ve belirtilen türde bir nesneye dönüştürür.</summary>
        /// <typeparam name="TKey">Dönüştürülecek nesne türü.</typeparam>
        /// <param name="form">Form koleksiyonu.</param>
        /// <param name="key">Anahtar adı.</param>
        /// <returns>Belirtilen türdeki değeri döndürür; anahtar bulunamazsa varsayılan değer döner.</returns>
        public static TKey ParseOrDefaultFromFormCollection<TKey>(this IFormCollection form, string key)
        {
            if (form.TryGetStringValue(key, out string _value)) { return _value.ParseOrDefault<TKey>(); }
            return default;
        }
        /// <summary>Form koleksiyonundan belirtilen anahtar ile bir dize değerini alır.</summary>
        /// <param name="form">Form koleksiyonu.</param>
        /// <param name="key">Anahtar adı.</param>
        /// <param name="outvalue">Dönüştürülen dize değeri.</param>
        /// <returns>Değer başarıyla alındıysa <see langword="true"/>, aksi takdirde <see langword="false"/> döner.</returns>
        public static bool TryGetStringValue(this IFormCollection form, string key, out string outvalue)
        {
            Guard.ThrowIfNull(form, nameof(form));
            if (form.TryGetValue(key.ToStringOrEmpty(), out StringValues _sv) && _sv.Count > 0)
            {
                outvalue = _sv.ToStringOrEmpty();
                return true;
            }
            outvalue = "";
            return false;
        }
        /// <summary>Bir IFormCollection nesnesinden belirtilen anahtara karşılık gelen değerleri belirli bir türde dizi olarak elde etmeye çalışır.</summary>
        /// <typeparam name="TKey">Dönüştürülmek istenen değerlerin türü.</typeparam>
        /// <param name="form">Değerin aranacağı IFormCollection nesnesi.</param>
        /// <param name="key">Hedef değerin anahtarı. Anahtarın &quot;[]&quot; ile bitmesi beklenir, aksi takdirde otomatik olarak eklenir.</param>
        /// <param name="outvalues">Belirtilen türdeki değerleri içeren çıktı dizisi. Anahtar bulunamazsa boş bir dizi döner.</param>
        /// <returns>Anahtar bulunduğunda ve değerler belirtilen türe dönüştürüldüğünde <see langword="true"/> döner, aksi takdirde <see langword="false"/> döner.</returns>
        /// <remarks>Bu metot, bir form verisindeki (IFormCollection) belirli bir anahtara karşılık gelen değerleri belirtilen türe dönüştürerek bir dizi olarak döndürmek için kullanılır. Eğer anahtar &quot;[]&quot; ile bitmiyorsa, otomatik olarak eklenir. Dönüştürme sırasında hata oluşursa, varsayılan değerler kullanılır.</remarks>
        public static bool TryGetArrayValue<TKey>(this IFormCollection form, string key, out TKey[] outvalues)
        {
            Guard.ThrowIfNull(form, nameof(form));
            key = key.ToStringOrEmpty();
            if (!key.EndsWith("[]")) { key = String.Concat(key, "[]"); }
            if (form.TryGetValue(key, out StringValues _sv) && _sv.Count > 0)
            {
                outvalues = _sv.Select(x => x.ParseOrDefault<TKey>()).ToArray();
                return true;
            }
            outvalues = [];
            return false;
        }
        /// <summary>
        /// Verilen <see cref="IFormCollection"/> verisini kullanarak belirtilen türde (<typeparamref name="T"/>) bir modeli model binding mekanizması ile oluşturmaya çalışır İşlem sırasında oluşan doğrulama hatalarını <see cref="ModelStateDictionary"/> üzerinden toplar ve sonuçla birlikte döner. Binding veya doğrulama hatası oluşursa hata mesajları ile birlikte başarısız sonucu; aksi durumda oluşturulan modeli döndürür.
        /// <para>Eğer <paramref name="httpContext"/> parametresi verilmezse, varsayılan bir <see cref="HttpContext"/> oluşturularak kullanılır.</para>
        /// </summary>
        /// <typeparam name="T">Binding işlemi sonucu oluşturulacak model tipi.</typeparam>
        /// <param name="form">Binding işlemi için kullanılacak form verileri.</param>
        /// <param name="httpContext">Opsiyonel <see cref="HttpContext"/> örneği.</param>
        /// <returns>
        /// Tuple olarak;
        /// <list type="bullet">
        /// <item><description><c>hasError</c>: İşlem sırasında hata oluşup oluşmadığını belirtir.</description></item>
        /// <item><description><c>model</c>: Başarılı ise oluşturulan model, aksi halde <c>null</c>.</description></item>
        /// <item><description><c>errors</c>: Oluşan hata mesajları.</description></item>
        /// </list>
        /// </returns>
        public static async Task<(bool hasError, T model, string[] errors)> TryBindFromFormAsync<T>(this IFormCollection form, HttpContext? httpContext = null) where T : class, new()
        {
            Guard.ThrowIfNull(form, nameof(form));
            httpContext ??= Utilities.GetDefaultHttpContext;
            var modelState = new ModelStateDictionary();
            var bindingContext = new DefaultModelBindingContext
            {
                ModelName = "",
                ValueProvider = new FormValueProvider(BindingSource.Form, form, CultureInfo.InvariantCulture),
                ModelState = modelState,
                ModelMetadata = httpContext.RequestServices.GetRequiredService<IModelMetadataProvider>().GetMetadataForType(typeof(T)),
                Model = new T(),
                IsTopLevelObject = true
            };
            var binder = httpContext.RequestServices.GetRequiredService<IModelBinderFactory>().CreateBinder(new ModelBinderFactoryContext
            {
                Metadata = bindingContext.ModelMetadata,
                BindingInfo = new BindingInfo { BindingSource = BindingSource.Form }
            });
            await binder.BindModelAsync(bindingContext);
            var errors = (modelState.IsValid ? [] : modelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).Distinct().ToArray());
            if (errors.Length > 0) { return (true, default, errors); }
            if (bindingContext.Result.IsModelSet && bindingContext.Result.Model is T _t)
            {
                if (TryValidators.TryValidateObject(_t, out errors)) { return (true, default, errors); }
                return (false, _t, default);
            }
            return (true, default, [GetDescriptionLocalizationValue(EnumResponseMessage.Error)]);
        }
        #endregion
        #region IFormFile
        /// <summary>Verilen IFormFile nesnesinden dosya adını döner (uzantısız).</summary>
        /// <param name="file">IFormFile nesnesi.</param>
        /// <returns>Dosya adı (uzantısız).</returns>
        public static string GetFileName(this IFormFile file) => Path.GetFileNameWithoutExtension(file.FileName);
        /// <summary>Verilen IFormFile nesnesinden dosya uzantısını döner (ilk karater &quot;.&quot; olacak biçimde)</summary>
        /// <param name="file">IFormFile nesnesi.</param>
        /// <returns>Dosya uzantısı (ilk karater &quot;.&quot; olacak biçimde).</returns>
        public static string GetFileExtension(this IFormFile file) => Path.GetExtension(file.FileName).ToLower();
        /// <summary>Verilen IFormFile nesnesini belirtilen fiziksel yola asenkron olarak yükler.</summary>
        public static async Task FileUpload(this IFormFile file, string physicallyPath, CancellationToken cancellationToken = default)
        {
            Guard.ThrowIfNull(file, nameof(file));
            Guard.ThrowIfEmpty(physicallyPath, nameof(physicallyPath));
            Files.DirectoryCreate(new FileInfo(physicallyPath).DirectoryName);
            using (var fs = new FileStream(physicallyPath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true))
            {
                await file.CopyToAsync(fs, cancellationToken);
            }
        }
        /// <summary>Bir IFormFile nesnesini byte dizisine dönüştürür.</summary>
        public static async Task<byte[]> ToByteArray(this IFormFile file, CancellationToken cancellationToken = default)
        {
            Guard.ThrowIfNull(file, nameof(file));
            using (var ms = new MemoryStream())
            {
                await file.CopyToAsync(ms, cancellationToken);
                return ms.ToArray();
            }
        }
        #endregion
        /// <summary><see cref="QueryString"/> içindeki belirtilen anahtarı alır ve uygun türde bir değere dönüştürür. Eğer anahtar bulunamazsa veya dönüştürme başarısız olursa, varsayılan değeri döner.</summary>
        /// <typeparam name="TKey">Dönüştürülecek hedef tür.</typeparam>
        /// <param name="queryString">İçinde sorgu parametrelerini barındıran <see cref="QueryString"/> nesnesi.</param>
        /// <param name="key">Alınacak sorgu parametresinin adı (anahtar).</param>
        /// <returns>Başarılıysa sorgu parametresi uygun türe dönüştürülür, aksi halde varsayılan değer döner.</returns>
        public static TKey ParseOrDefaultFromQueryString<TKey>(this QueryString queryString, string key)
        {
            var querydic = (queryString.HasValue ? HttpUtility.ParseQueryString(queryString.Value) : []);
            key = key.ToStringOrEmpty();
            if (querydic.AllKeys.Contains(key)) { return querydic[key].ParseOrDefault<TKey>(); }
            return default;
        }
        /// <summary>ModelStateDictionary nesnesine bir dizi hata mesajını topluca eklemek için kullanılan bir genişletme metodu.</summary>
        /// <param name="modelstate">Hataların ekleneceği ModelStateDictionary nesnesi.</param>
        /// <param name="errors">Eklenecek hata mesajlarını içeren string listesi.</param>
        /// <remarks>Bu metot, verilen hata mesajları listesindeki her bir öğeyi ModelState&#39;e tek tek ekler. Key olarak boş bir string (&quot;&quot;) kullanılır. Eğer <paramref name="errors"/> null ise işlem yapılmaz.</remarks>
        public static void AddModelErrorRange(this ModelStateDictionary modelstate, IEnumerable<string> errors)
        {
            if (!errors.IsNullOrEmptyOrAllNull())
            {
                Guard.ThrowIfNull(modelstate, nameof(modelstate));
                foreach (var item in errors.Distinct().ToArray()) { modelstate.AddModelError("_", item); }
            }
        }
    }
}