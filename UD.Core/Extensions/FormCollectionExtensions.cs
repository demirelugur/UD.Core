namespace UD.Core.Extensions
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Primitives;
    using System.Globalization;
    using UD.Core.Helper;
    using UD.Core.Helper.Validation;
    using static UD.Core.Enums.CRetMesaj;
    public static class FormCollectionExtensions
    {
        /// <summary>Belirtilen anahtar ile form verilerinden bir değeri alır ve belirtilen türde bir nesneye dönüştürür.</summary>
        /// <typeparam name="TKey">Dönüştürülecek nesne türü.</typeparam>
        /// <param name="form">Form koleksiyonu.</param>
        /// <param name="key">Anahtar adı.</param>
        /// <returns>Belirtilen türdeki değeri döndürür; anahtar bulunamazsa varsayılan değer döner.</returns>
        public static TKey ParseOrDefault<TKey>(this IFormCollection form, string key)
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
            Guard.ThrowIfEmpty(key, nameof(key));
            if (form.TryGetValue(key, out StringValues _sv) && _sv.Count > 0)
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
            Guard.ThrowIfEmpty(key, nameof(key));
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
        /// <br/>
        /// Eğer <paramref name="httpContext"/> parametresi verilmezse, varsayılan bir <see cref="HttpContext"/> oluşturularak kullanılır.
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
            var model = new T();
            var modelState = new ModelStateDictionary();
            var bindingContext = new DefaultModelBindingContext
            {
                ModelName = typeof(T).Name,
                ValueProvider = new FormValueProvider(BindingSource.Form, form, CultureInfo.CurrentCulture),
                ModelState = modelState,
                ModelMetadata = httpContext.RequestServices.GetRequiredService<IModelMetadataProvider>().GetMetadataForType(typeof(T)),
                Model = model,
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
            if (bindingContext.Result.IsModelSet && bindingContext.Result.Model is T _t) { return (false, _t, default); }
            return (true, default, [GetDescriptionLocalizationValue(RetMesaj.hata)]);
        }
    }
}