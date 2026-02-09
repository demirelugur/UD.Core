namespace UD.Core.Extensions
{
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UD.Core.Helper.Results;
    public static class CollectionExtensions
    {
        #region IDictionary
        /// <summary>
        /// Belirtilen anahtar ve değeri bir sözlüğe ekler. Eğer anahtar zaten mevcutsa, değeri günceller; mevcut değilse, yeni bir anahtar-değer çifti olarak ekler.
        /// </summary>
        /// <typeparam name="K">Sözlükteki anahtar türü.</typeparam>
        /// <typeparam name="V">Sözlükteki değer türü.</typeparam>
        /// <param name="dictionary">Üzerinde işlem yapılacak sözlük.</param>
        /// <param name="key">Eklenecek veya güncellenecek anahtar.</param>
        /// <param name="value">Anahtarla ilişkilendirilecek değer.</param>
        public static void AddOrUpdate<K, V>(this IDictionary<K, V> dictionary, K key, V value)
        {
            if (!dictionary.TryAdd(key, value)) { dictionary[key] = value; }
        }
        /// <summary>
        /// Verilen koşula uyan tüm anahtar-değer çiftlerini sözlükten kaldırır.
        /// </summary>
        /// <typeparam name="K">Anahtar tipi.</typeparam>
        /// <typeparam name="V">Değer tipi.</typeparam>
        /// <param name="dictionary">Anahtar-değer çiftlerinin bulunduğu sözlük.</param>
        /// <param name="predicate">Kaldırılacak öğeleri belirleyen bir koşul.</param>
        public static void RemoveWhere<K, V>(this IDictionary<K, V> dictionary, Func<KeyValuePair<K, V>, bool> predicate)
        {
            foreach (var itemkey in dictionary.Where(predicate).Select(x => x.Key).ToArray()) { dictionary.Remove(itemkey); }
        }
        /// <summary>
        /// Belirtilen anahtara (key) göre bir sözlükten (IDictionary) değer çekip, belirli bir türe <typeparamref name="TKey"/> dönüştürür. Eğer sözlük veya anahtar geçersizse, varsayılan değeri döndürür.
        /// </summary>
        /// <typeparam name="TKey">Dönüştürülecek veri türü.</typeparam>
        /// <param name="dic">Anahtar - Değer çiftleri içeren sözlük.</param>
        /// <param name="key">Sözlükte aranan anahtar (key).</param>
        /// <returns>
        /// Sözlükte belirtilen anahtara karşılık gelen değeri <typeparamref name="TKey"/> türüne dönüştürülmüş şekilde döndürür. Anahtar yoksa veya geçersizse, <typeparamref name="TKey"/> türünün varsayılan değerini döndürür.
        /// </returns>
        public static TKey ParseOrDefault<TKey>(this IDictionary<string, string> dic, string key)
        {
            if (!key.IsNullOrEmpty())
            {
                dic = dic ?? new Dictionary<string, string>();
                if (dic.TryGetValue(key, out string _value)) { return _value.ParseOrDefault<TKey>(); }
            }
            return default;
        }
        /// <summary>
        /// ModelStateDictionary nesnesine bir dizi hata mesajını topluca eklemek için kullanılan bir genişletme metodu.
        /// </summary>
        /// <param name="modelstate">Hataların ekleneceği ModelStateDictionary nesnesi.</param>
        /// <param name="errors">Eklenecek hata mesajlarını içeren string listesi.</param>
        /// <remarks>
        /// Bu metot, verilen hata mesajları listesindeki her bir öğeyi
        /// ModelState&#39;e tek tek ekler. Key olarak boş bir string (&quot;&quot;) kullanılır.
        /// Eğer <paramref name="errors"/> null ise işlem yapılmaz.
        /// </remarks>
        public static void AddModelErrorRange(this ModelStateDictionary modelstate, IEnumerable<string> errors)
        {
            if (errors != null) { foreach (var item in errors.Distinct().ToArray()) { modelstate.AddModelError("", item); } }
        }
        #endregion
        #region IEnumerable
        /// <summary>
        /// Verilen koleksiyondaki tüm nesneleri temizler ve dispose eder.
        /// </summary>
        /// <param name="values">Dispose edilecek nesneleri içeren koleksiyon.</param>
        public static void DisposeAll<T>(this IEnumerable<T> values) where T : IDisposable
        {
            if (values != null) { foreach (var value in values) { value.Dispose(); } }
        }
        /// <summary>
        /// İki koleksiyon arasında bir sol dış birleşim (left join) gerçekleştiren bir yöntemdir. Her öğe için bir anahtar kullanır ve sağdaki koleksiyondan bir eşleşme olup olmadığını kontrol eder.
        /// </summary>
        /// <typeparam name="TLeft">Sol koleksiyon tipi.</typeparam>
        /// <typeparam name="TRight">Sağ koleksiyon tipi.</typeparam>
        /// <typeparam name="TKey">Anahtar tipi.</typeparam>
        /// <param name="left">Sol koleksiyon.</param>
        /// <param name="right">Sağ koleksiyon.</param>
        /// <param name="leftkey">Sol koleksiyondaki anahtar için bir seçici.</param>
        /// <param name="rightkey">Sağ koleksiyondaki anahtar için bir seçici.</param>
        /// <returns>Sol dış birleşim sonucu içeren bir IEnumerable.</returns>
        public static IEnumerable<LeftJoinResult<TLeft, TRight>> LeftJoinEnumerable<TLeft, TRight, TKey>(this IEnumerable<TLeft> left, IEnumerable<TRight> right, Func<TLeft, TKey> leftkey, Func<TRight, TKey> rightkey) where TLeft : class where TRight : class => left.GroupJoin(right, leftkey, rightkey, (l, r) => new
        {
            l,
            r
        }).SelectMany(x => x.r.DefaultIfEmpty(), (l, r) => new LeftJoinResult<TLeft, TRight>
        {
            left = l.l,
            hasright = r != null,
            right = r
        });
        /// <summary>
        /// Kaynakdaki elemanların sırasını <b>Fisher-Yates algoritmasını</b> kullanarak rastgele karıştırır ve karıştırılmış bir ICollection olarak geri döner.
        /// </summary>
        /// <typeparam name="T">Kaynağın eleman türü.</typeparam>
        /// <param name="source">Rastgele sıralanacak orijinal kaynak.</param>
        /// <returns>Karıştırılmış elemanları içeren yeni bir ICollection&lt;T&gt; örneği.</returns>
        /// <remarks>
        /// Bu metot, verilen IEnumerable&lt;T&gt; kaynakdaki elemanların yerini <b>Fisher-Yates algoritması</b> ile rastgele değiştirir. Karıştırılmış elemanları yeni bir ICollection&lt;T&gt; olarak döndürür. &quot;Random.Shared&quot; ile tek bir Random örneği paylaşılır, bu da çoklu iş parçacıklı senaryolarda daha güvenilir bir kullanım sağlar.
        /// </remarks>
        public static ICollection<T> Shuffle<T>(this IEnumerable<T> source)
        {
            T _temp;
            var _r = source.ToList();
            int i, j, _count = (_r.Count - 1);
            for (i = _count; i > 0; i--)
            {
                j = Random.Shared.Next(0, i + 1);
                _temp = _r[i];
                _r[i] = _r[j];
                _r[j] = _temp;
            }
            return _r;
        }
        #endregion
        #region ICollection
        /// <summary>
        /// İki koleksiyonun eşitliğini kontrol eder. Eşitlik, koleksiyonların aynı öğeleri içermesine ve aynı sayıda öğeye sahip olmasına bağlıdır. Öğelerin sıralaması dikkate alınmaz; [1, 2, 3, 4, 5] ve [4, 3, 2, 5, 1] eşit kabul edilir.
        /// </summary>
        /// <typeparam name="T">Koleksiyonun öğelerinin türü.</typeparam>
        /// <param name="left">Karşılaştırılacak ilk koleksiyon.</param>
        /// <param name="right">Karşılaştırılacak ikinci koleksiyon.</param>
        /// <returns>İki koleksiyon eşitse <see langword="true"/>, aksi takdirde <see langword="false"/> döner.</returns>
        public static bool IsEqual<T>(this ICollection<T> left, ICollection<T> right)
        {
            var _leftisnull = left == null;
            var _rightisnull = right == null;
            if (((_leftisnull || _rightisnull) && _leftisnull == _rightisnull)) { return true; }
            if (!_leftisnull && !_rightisnull && left.Count == right.Count && left.All(right.Contains)) { return true; }
            return false;
        }
        /// <summary>
        /// Verilen koleksiyonun boş veya null olup olmadığını kontrol eder.
        /// </summary>
        /// <typeparam name="T">Koleksiyon tipi.</typeparam>
        /// <param name="values">Kontrol edilecek koleksiyon.</param>
        /// <returns>Boş veya null ise <see langword="true"/>, aksi halde <see langword="false"/> döner.</returns>
        public static bool IsNullOrCountZero<T>(this ICollection<T> values) => (values == null || values.Count == 0 || values.All(x => x == null));
        /// <summary>
        /// Başka bir koleksiyondan mevcut koleksiyona öğeleri topluca ekler. <see cref="List{T}"/> için optimize edilmiş bir yöntemdir.
        /// </summary>
        /// <typeparam name="T">Koleksiyon tipi.</typeparam>
        /// <param name="initial">Öğelerin ekleneceği mevcut koleksiyon.</param>
        /// <param name="other">Eklenecek öğeleri içeren diğer koleksiyon.</param>
        public static void AddRangeOptimized<T>(this ICollection<T> initial, IEnumerable<T> other)
        {
            if (other != null && other.Any())
            {
                if (initial is List<T> _l) { _l.AddRange(other); }
                else { foreach (var l in other) { initial.Add(l); } }
            }
        }
        #endregion
        #region string[]
        /// <summary>
        /// Hata mesajları dizisini iç içe geçmiş istisnalara dönüştürür.
        /// </summary>
        /// <param name="errors">Hata mesajlarının yer aldığı dizi.</param>
        /// <returns>İç içe geçmiş <see cref="Exception"/> nesnesi.</returns>
        public static Exception ToNestedException(this string[] errors)
        {
            errors = (errors ?? Array.Empty<string>()).Reverse().ToArray();
            if (errors.Length == 0) { return new(); }
            Exception _ex = null;
            var _i = errors.Length - 1;
            while (_i >= 0)
            {
                if (_ex == null) { _ex = new(errors[_i]); }
                else { _ex = new(errors[_i], _ex); }
                _i--;
            }
            return _ex;
        }
        #endregion
    }
}