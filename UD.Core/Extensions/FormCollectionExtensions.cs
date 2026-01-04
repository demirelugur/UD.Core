namespace UD.Core.Extensions
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Primitives;
    public static class FormCollectionExtensions
    {
        /// <summary>
        /// Belirtilen anahtar ile form verilerinden bir değeri alır ve belirtilen türde bir nesneye dönüştürür.
        /// </summary>
        /// <typeparam name="TKey">Dönüştürülecek nesne türü.</typeparam>
        /// <param name="form">Form koleksiyonu.</param>
        /// <param name="key">Anahtar adı.</param>
        /// <returns>Belirtilen türdeki değeri döndürür; anahtar bulunamazsa varsayılan değer döner.</returns>
        public static TKey ParseOrDefault<TKey>(this IFormCollection form, string key)
        {
            if (form.TryGetStringValue(key, out string _value)) { return _value.ParseOrDefault<TKey>(); }
            return default;
        }
        /// <summary>
        /// Form koleksiyonundan belirtilen anahtar ile bir dize değerini alır.
        /// </summary>
        /// <param name="form">Form koleksiyonu.</param>
        /// <param name="key">Anahtar adı.</param>
        /// <param name="outvalue">Dönüştürülen dize değeri.</param>
        /// <returns>Değer başarıyla alındıysa <see langword="true"/>, aksi takdirde <see langword="false"/> döner.</returns>
        public static bool TryGetStringValue(this IFormCollection form, string key, out string outvalue)
        {
            if (form == null) { form = FormCollection.Empty; }
            var _r = form.TryGetValue(key, out StringValues _sv);
            if (_r)
            {
                outvalue = _sv.ToStringOrEmpty();
                return true;
            }
            outvalue = "";
            return false;
        }
        /// <summary>
        /// Bir IFormCollection nesnesinden belirtilen anahtara karşılık gelen değerleri belirli bir türde dizi olarak elde etmeye çalışır.
        /// </summary>
        /// <typeparam name="TKey">Dönüştürülmek istenen değerlerin türü.</typeparam>
        /// <param name="form">Değerin aranacağı IFormCollection nesnesi.</param>
        /// <param name="key">Hedef değerin anahtarı. Anahtarın &quot;[]&quot; ile bitmesi beklenir, aksi takdirde otomatik olarak eklenir.</param>
        /// <param name="outvalues">Belirtilen türdeki değerleri içeren çıktı dizisi. Anahtar bulunamazsa boş bir dizi döner.</param>
        /// <returns>
        /// Anahtar bulunduğunda ve değerler belirtilen türe dönüştürüldüğünde <see langword="true"/> döner, aksi takdirde <see langword="false"/> döner.
        /// </returns>
        /// <remarks>
        /// Bu metot, bir form verisindeki (IFormCollection) belirli bir anahtara karşılık gelen değerleri belirtilen türe dönüştürerek bir dizi olarak döndürmek için kullanılır. Eğer anahtar &quot;[]&quot; ile bitmiyorsa, otomatik olarak eklenir. Dönüştürme sırasında hata oluşursa, varsayılan değerler kullanılır.
        /// </remarks>
        public static bool TryGetArrayValue<TKey>(this IFormCollection form, string key, out TKey[] outvalues)
        {
            key = key.ToStringOrEmpty();
            if (key != "")
            {
                if (!key.EndsWith("[]")) { key = String.Concat(key, "[]"); }
                if (form == null) { form = FormCollection.Empty; }
                if (form.TryGetValue(key, out StringValues _sv))
                {
                    outvalues = _sv.Select(x => x.ParseOrDefault<TKey>()).ToArray();
                    return true;
                }
            }
            outvalues = Array.Empty<TKey>();
            return false;
        }
    }
}