namespace UD.Core.Extensions
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Query;
    using System;
    using System.Linq;
    using System.Linq.Dynamic.Core;
    using System.Linq.Expressions;
    using UD.Core.Helper.Configuration;
    using UD.Core.Helper.Paging;
    using UD.Core.Helper.Results;
    using UD.Core.Helper.Validation;
    public static class IQueryableExtensions
    {
        /// <summary>Belirtilen koşul sağlandığında sorguya ek filtre uygular. Dinamik olarak filtre eklemek istediğiniz durumlarda kullanışlıdır.</summary>
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, bool condition, Expression<Func<T, bool>> predicate)
        {
            if (condition) { return source.Where(predicate); }
            return source;
        }
        /// <summary>IQueryable kaynağını asenkron olarak diziye çevirir. EF Core destekliyorsa ToArrayAsync, değilse ToArray kullanır.</summary>
        /// <typeparam name="T">Eleman tipi</typeparam>
        /// <param name="source">Kaynak sorgu</param>
        /// <param name="cancellationToken">İptal token&#39;ı</param>
        public static Task<T[]> ToArraySafe<T>(this IQueryable<T> source, CancellationToken cancellationToken = default)
        {
            if (source.Provider is IAsyncQueryProvider) { return source.ToArrayAsync(cancellationToken); }
            return Task.FromResult(source.ToArray());
        }
        /// <summary>İki IQueryable arasında 1-1 sol birleştirme (left join) işlemi gerçekleştirir.</summary>
        /// <typeparam name="TLeft">Sol taraftaki nesne türü.</typeparam>
        /// <typeparam name="TRight">Sağ taraftaki nesne türü.</typeparam>
        /// <typeparam name="TKey">Birleştirme için anahtar türü.</typeparam>
        /// <param name="left">Sol IQueryable kaynak.</param>
        /// <param name="right">Sağ IQueryable kaynak.</param>
        /// <param name="leftKey">Sol kaynak için anahtar ifadesi.</param>
        /// <param name="rightKey">Sağ kaynak için anahtar ifadesi.</param>
        /// <returns>Birleştirilmiş sonuçları içeren IQueryable kaynak.</returns>
        public static IQueryable<LeftJoinResult<TLeft, TRight>> LeftJoinQueryable<TLeft, TRight, TKey>(this IQueryable<TLeft> left, IQueryable<TRight> right, Expression<Func<TLeft, TKey>> leftKey, Expression<Func<TRight, TKey>> rightKey) where TLeft : class where TRight : class => left.GroupJoin(right, leftKey, rightKey, (l, r) => new
        {
            l,
            r
        }).SelectMany(x => x.r.DefaultIfEmpty(), (l, r) => new LeftJoinResult<TLeft, TRight>
        {
            left = l.l,
            hasRight = r != null,
            right = r
        });
        /// <summary>Seçilen ifadeye göre ilk kayıt veya varsayılan değeri asenkron olarak getirir.</summary>
        public static Task<TObject> SelectThenFirstOrDefault<T, TObject>(this IQueryable<T> source, Expression<Func<T, TObject>> selector, CancellationToken cancellationToken = default) where T : class => source.Select(selector).FirstOrDefaultAsync(cancellationToken);
        /// <summary>Verilen ifade ile maksimum değeri asenkron olarak getirir, yoksa varsayılan değeri döner.</summary>
        public static async Task<TKey> MaxOrDefault<T, TKey>(this IQueryable<T> source, Expression<Func<T, TKey>> selector, CancellationToken cancellationToken = default) where T : class => (await source.AnyAsync(cancellationToken) ? await source.MaxAsync(selector, cancellationToken) : default(TKey));
        /// <summary>Verilen ifade ile minimum değeri asenkron olarak getirir, yoksa varsayılan değeri döner.</summary>
        public static async Task<TKey> MinOrDefault<T, TKey>(this IQueryable<T> source, Expression<Func<T, TKey>> selector, CancellationToken cancellationToken = default) where T : class => (await source.AnyAsync(cancellationToken) ? await source.MinAsync(selector, cancellationToken) : default(TKey));
        /// <summary>Verilen metin için benzersiz bir SEO dostu string oluşturur.</summary>
        public static async Task<string> GenerateUniqueSEOString(this IQueryable<string> source, string text, int maxLength, string dil, CancellationToken cancellationToken = default)
        {
            var i = 0;
            string r, textSeo = text.ToSeoFriendly();
            Guard.CheckEmpty(textSeo, nameof(textSeo));
            while (true)
            {
                r = (i == 0 ? textSeo : String.Join("-", textSeo, i.ToString()));
                if (!await source.ContainsAsync(r, cancellationToken)) { break; }
                i++;
            }
            if (r.Length <= maxLength) { return r; }
            Guard.UnSupportLanguage(dil, nameof(dil));
            if (dil == "en") { throw new ArgumentOutOfRangeException($"The generated SEO data exceeds the maximum length of {maxLength} characters!"); }
            throw new ArgumentOutOfRangeException($"Oluşturulan SEO verisi {maxLength} karakterlik maksimum uzunluğu aşıyor!");
        }
        /// <summary> IQueryable koleksiyonunu asenkron olarak sayfalanmış bir listeye dönüştürür. </summary>
        /// <typeparam name="T">Sorgu sonucundaki öğelerin tipi.</typeparam>
        /// <param name="source">Sayfalanacak IQueryable veri kaynağı.</param>
        /// <param name="pageNumber">İstenen sayfa numarası. (1 tabanlı)</param>
        /// <param name="size">Sayfa başına öğe sayısı.</param>
        /// <param name="sorting">Sorgu sıralaması</param>
        /// <param name="loadInfo">Sayfalama bilgilerinin (toplam sayfa, toplam öğe sayısı vb.) yüklenip yüklenmeyeceğini belirtir. Varsayılan değer: <see langword="true"/>.</param>
        /// <param name="cancellationToken">Asenkron işlemi iptal etmek için kullanılan token.</param>
        public static async Task<Paginate<T>> ToPagedList<T>(this IQueryable<T> source, int pageNumber, int size, string sorting, bool loadInfo = true, CancellationToken cancellationToken = default)
        {
            if (source == null) { return new(); }
            PagingInfo? p = null;
            if (loadInfo)
            {
                var totalcount = await source.LongCountAsync(cancellationToken);
                var totalpage = Convert.ToInt64(Math.Ceiling(totalcount / Convert.ToDouble(size)));
                p = new(totalcount, totalpage, pageNumber);
            }
            IOrderedQueryable<T> orderedSource;
            if (sorting.IsNullOrEmpty())
            {
                if (typeof(T).IsSubclassOfOpenGeneric(typeof(EntityDto<>))) { orderedSource = source.OrderBy(nameof(EntityDto<>.Id)); }
                else { orderedSource = source.OrderBy(x => 0); }
            }
            else
            {
                try { orderedSource = source.OrderBy(sorting); }
                catch (Exception ex) { throw new InvalidOperationException($"Sorting failed: {sorting}", ex); }
            }
            var items = await orderedSource.Paginate(pageNumber, size).ToArrayAsync(cancellationToken);
            return new(pageNumber, size, items, p);
        }
        /// <summary>
        /// IOrderedQueryable kaynaklarının sayfalama işlemini gerçekleştirir.
        /// <para>
        /// <example>
        /// SQL Örneği: <br/>
        /// SELECT * FROM [dbo].[LoremIpsum] ORDER BY [Key] OFFSET ((@pagenumber - 1) * @size) ROWS FETCH NEXT @size ROWS ONLY
        /// </example>
        /// </para>
        /// </summary>
        /// <param name="source">Sayfalama işlemi yapılacak IOrderedQueryable kaynağı.</param>
        /// <param name="pageNumber">Sayfa numarası (1 tabanlı).</param>
        /// <param name="size">Her sayfada gösterilecek kayıt sayısı.</param>
        /// <returns>Paginasyon yapılmış IQueryable kaynak.</returns>
        public static IQueryable<T> Paginate<T>(this IOrderedQueryable<T> source, int pageNumber, int size)
        {
            pageNumber = Math.Max(1, pageNumber);
            size = Math.Max(1, size);
            if (pageNumber == 1) { return source.Take(size); }
            var skip = ((pageNumber - 1) * size);
            return source.Skip(skip).Take(size);
        }
    }
}