namespace UD.Core.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UD.Core.Helper.Results;
    using UD.Core.Helper.Validation;
    public static class GenericExtensions
    {
        #region IDictionary
        /// <summary>Belirtilen anahtar ve değeri bir sözlüğe ekler. Eğer anahtar zaten mevcutsa, değeri günceller; mevcut değilse, yeni bir anahtar-değer çifti olarak ekler.</summary>
        /// <typeparam name="K">Sözlükteki anahtar türü.</typeparam>
        /// <typeparam name="V">Sözlükteki değer türü.</typeparam>
        /// <param name="dictionary">Üzerinde işlem yapılacak sözlük.</param>
        /// <param name="key">Eklenecek veya güncellenecek anahtar.</param>
        /// <param name="value">Anahtarla ilişkilendirilecek değer.</param>
        public static void AddOrUpdate<K, V>(this IDictionary<K, V> dictionary, K key, V value)
        {
            if (!dictionary.TryAdd(key, value)) { dictionary[key] = value; }
        }
        /// <summary>Verilen koşula uyan tüm anahtar - değer çiftlerini sözlükten kaldırır.</summary>
        /// <typeparam name="K">Anahtar tipi.</typeparam>
        /// <typeparam name="V">Değer tipi.</typeparam>
        /// <param name="dictionary">Anahtar-değer çiftlerinin bulunduğu sözlük.</param>
        /// <param name="predicate">Kaldırılacak öğeleri belirleyen bir koşul.</param>
        public static void RemoveWhere<K, V>(this IDictionary<K, V> dictionary, Func<KeyValuePair<K, V>, bool> predicate)
        {
            foreach (var itemKey in dictionary.Where(predicate).Select(x => x.Key).ToArray()) { dictionary.Remove(itemKey); }
        }
        /// <summary>Belirtilen anahtara (key) göre bir sözlükten (IDictionary) değer çekip, belirli bir türe <typeparamref name="TKey"/> dönüştürür. Eğer sözlük veya anahtar geçersizse, varsayılan değeri döndürür.</summary>
        /// <typeparam name="TKey">Dönüştürülecek veri türü.</typeparam>
        /// <param name="dictionary">Anahtar - Değer çiftleri içeren sözlük.</param>
        /// <param name="key">Sözlükte aranan anahtar (key).</param>
        /// <returns>Sözlükte belirtilen anahtara karşılık gelen değeri <typeparamref name="TKey"/> türüne dönüştürülmüş şekilde döndürür. Anahtar yoksa veya geçersizse, <typeparamref name="TKey"/> türünün varsayılan değerini döndürür.</returns>
        public static TKey ParseOrDefault<TKey>(this IDictionary<string, object> dictionary, string key)
        {
            dictionary ??= new Dictionary<string, object>();
            if (dictionary.TryGetValue(key.ToStringOrEmpty(), out object _value)) { return _value.ParseOrDefault<TKey>(); }
            return default;
        }
        #endregion
        #region IEnumerable
        /// <summary>Verilen koleksiyondaki tüm nesneleri temizler ve dispose eder.</summary>
        /// <param name="source">Dispose edilecek nesneleri içeren koleksiyon.</param>
        public static void DisposeAll<T>(this IEnumerable<T> source) where T : IDisposable
        {
            if (source != null) { foreach (var value in source) { value.Dispose(); } }
        }
        /// <summary>İki koleksiyon arasında bir sol dış birleşim (left join) gerçekleştiren bir yöntemdir. Her öğe için bir anahtar kullanır ve sağdaki koleksiyondan bir eşleşme olup olmadığını kontrol eder.</summary>
        /// <typeparam name="TLeft">Sol koleksiyon tipi.</typeparam>
        /// <typeparam name="TRight">Sağ koleksiyon tipi.</typeparam>
        /// <typeparam name="TKey">Anahtar tipi.</typeparam>
        /// <param name="left">Sol koleksiyon.</param>
        /// <param name="right">Sağ koleksiyon.</param>
        /// <param name="leftKey">Sol koleksiyondaki anahtar için bir seçici.</param>
        /// <param name="rightKey">Sağ koleksiyondaki anahtar için bir seçici.</param>
        /// <returns>Sol dış birleşim sonucu içeren bir IEnumerable.</returns>
        public static IEnumerable<LeftJoinResult<TLeft, TRight>> LeftJoinEnumerable<TLeft, TRight, TKey>(this IEnumerable<TLeft> left, IEnumerable<TRight> right, Func<TLeft, TKey> leftKey, Func<TRight, TKey> rightKey) where TLeft : class where TRight : class => left.GroupJoin(right, leftKey, rightKey, (l, r) => new
        {
            l,
            r
        }).SelectMany(x => x.r.DefaultIfEmpty(), (l, r) => new LeftJoinResult<TLeft, TRight>
        {
            left = l.l,
            hasRight = r != null,
            right = r
        });
        /// <summary><paramref name="source"/> koleksiyonunun null, boş veya tüm öğelerinin null olup olmadığını kontrol eder. Koleksiyon null ise, boş ise veya tüm öğeleri null ise <see langword="true"/> döner; aksi takdirde <see langword="false"/> döner.</summary>
        public static bool IsNullOrEmptyOrAllNull<T>(this IEnumerable<T> source)
        {
            if (source == null) { return true; } // Alternatif: (source == null || !source.Any() || source.All(x => x == null))
            using (var enumerator = source.GetEnumerator())
            {
                if (!enumerator.MoveNext()) { return true; }
                do { if (enumerator.Current != null) { return false; } }
                while (enumerator.MoveNext());
            }
            return true;
        }
        /// <summary><paramref name="objA"/> ve <paramref name="objB"/> koleksiyonlarının sırasız olarak eşit olup olmadığını kontrol eder. İki koleksiyonun aynı öğeleri içerip içermediğini, ancak sıralarının önemli olmadığı durumlarda kullanılır. Her iki koleksiyonun da aynı öğeleri içerdiği, ancak farklı sıralarda olabilirlerse <see langword="true"/> döner; aksi takdirde <see langword="false"/> döner.</summary>
        public static bool IsUnorderedEqual<T>(this IEnumerable<T> objA, IEnumerable<T> objB)
        {
            if (ReferenceEquals(objA, objB)) { return true; }
            if (objA == null || objB == null) { return false; }
            var objASet = objA.ToHashSet();
            var objBSet = objB.ToHashSet();
            return objASet.SetEquals(objBSet);
        }
        #endregion
        #region ICollection
        /// <summary>Başka bir koleksiyondan mevcut koleksiyona öğeleri topluca ekler. <see cref="List{T}"/> için optimize edilmiş bir yöntemdir.</summary>
        /// <typeparam name="T">Koleksiyon tipi.</typeparam>
        /// <param name="initial">Öğelerin ekleneceği mevcut koleksiyon.</param>
        /// <param name="other">Eklenecek öğeleri içeren diğer koleksiyon.</param>
        public static void AddRangeOptimized<T>(this ICollection<T> initial, IEnumerable<T> other)
        {
            Guard.ThrowIfNull(initial, nameof(initial));
            if (!other.IsNullOrEmptyOrAllNull())
            {
                if (initial is List<T> _l) { _l.AddRange(other); }
                else { foreach (var item in other) { initial.Add(item); } }
            }
        }
        #endregion
    }
}