namespace UD.Core.Extensions
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using UD.Core.Helper;
    using UD.Core.Helper.Paging;
    using UD.Core.Helper.Results;
    public static class IQueryableExtensions
    {
        /// <summary>Belirtilen koşul sağlandığında sorguya ek filtre uygular. Dinamik olarak filtre eklemek istediğiniz durumlarda kullanışlıdır.</summary>
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, bool condition, Expression<Func<T, bool>> predicate)
        {
            if (condition) { return source.Where(predicate); }
            return source;
        }
        /// <summary>
        /// IQueryable kaynaklarının sayfalama işlemini gerçekleştirir.
        /// <para>
        /// <example>
        /// SQL Örneği: <br/>
        /// SELECT * FROM [dbo].[LoremIpsum] ORDER BY [Key] OFFSET ((@pagenumber - 1) * @pagesize) ROWS FETCH NEXT @pagesize ROWS ONLY
        /// </example>
        /// </para>
        /// </summary>
        /// <param name="source">Sayfalama işlemi yapılacak IQueryable kaynağı.</param>
        /// <param name="pagenumber">Sayfa numarası (1 tabanlı).</param>
        /// <param name="pagesize">Her sayfada gösterilecek kayıt sayısı.</param>
        /// <returns>Paginasyon yapılmış IQueryable kaynak.</returns>
        public static IQueryable<T> Paginate<T>(this IQueryable<T> source, int pagenumber, int pagesize)
        {
            Guard.CheckZeroOrNegative(pagenumber, nameof(pagenumber));
            return source.Skip((pagenumber - 1) * pagesize).Take(pagesize);
        }
        /// <summary>İki IQueryable arasında 1-1 sol birleştirme (left join) işlemi gerçekleştirir.</summary>
        /// <typeparam name="TLeft">Sol taraftaki nesne türü.</typeparam>
        /// <typeparam name="TRight">Sağ taraftaki nesne türü.</typeparam>
        /// <typeparam name="TKey">Birleştirme için anahtar türü.</typeparam>
        /// <param name="left">Sol IQueryable kaynak.</param>
        /// <param name="right">Sağ IQueryable kaynak.</param>
        /// <param name="leftkey">Sol kaynak için anahtar ifadesi.</param>
        /// <param name="rightkey">Sağ kaynak için anahtar ifadesi.</param>
        /// <returns>Birleştirilmiş sonuçları içeren IQueryable kaynak.</returns>
        public static IQueryable<LeftJoinResult<TLeft, TRight>> LeftJoinQueryable<TLeft, TRight, TKey>(this IQueryable<TLeft> left, IQueryable<TRight> right, Expression<Func<TLeft, TKey>> leftkey, Expression<Func<TRight, TKey>> rightkey) where TLeft : class where TRight : class => left.GroupJoin(right, leftkey, rightkey, (l, r) => new
        {
            l,
            r
        }).SelectMany(x => x.r.DefaultIfEmpty(), (l, r) => new LeftJoinResult<TLeft, TRight>
        {
            left = l.l,
            hasright = r != null,
            right = r
        });
        /// <summary>Seçilen ifadeye göre ilk kayıt veya varsayılan değeri asenkron olarak getirir.</summary>
        public static Task<TObject> SelectThenFirstOrDefaultAsync<T, TObject>(this IQueryable<T> source, Expression<Func<T, TObject>> selector, CancellationToken cancellationtoken = default) where T : class => source.Select(selector).FirstOrDefaultAsync(cancellationtoken);
        /// <summary>Verilen ifade ile maksimum değeri asenkron olarak getirir, yoksa varsayılan değeri döner.</summary>
        public static async Task<TKey> MaxOrDefaultAsync<T, TKey>(this IQueryable<T> source, Expression<Func<T, TKey>> selector, CancellationToken cancellationtoken = default) where T : class => (await source.AnyAsync(cancellationtoken) ? await source.MaxAsync(selector, cancellationtoken) : default(TKey));
        /// <summary>Verilen ifade ile minimum değeri asenkron olarak getirir, yoksa varsayılan değeri döner.</summary>
        public static async Task<TKey> MinOrDefaultAsync<T, TKey>(this IQueryable<T> source, Expression<Func<T, TKey>> selector, CancellationToken cancellationtoken = default) where T : class => (await source.AnyAsync(cancellationtoken) ? await source.MinAsync(selector, cancellationtoken) : default(TKey));
        /// <summary>Verilen metin için benzersiz bir SEO dostu string oluşturur.</summary>
        public static async Task<string> GenerateUniqueSEOStringAsync(this IQueryable<string> source, string text, int maxlength, string dil, CancellationToken cancellationtoken = default)
        {
            var _i = 0;
            string _r, _t = text.ToSeoFriendly();
            Guard.CheckEmpty(_t, nameof(_t));
            while (true)
            {
                _r = (_i == 0 ? _t : String.Join("-", _t, _i.ToString()));
                if (!await source.ContainsAsync(_r, cancellationtoken)) { break; }
                _i++;
            }
            if (_r.Length <= maxlength) { return _r; }
            Guard.UnSupportLanguage(dil, nameof(dil));
            if (dil == "en") { throw new ArgumentOutOfRangeException($"The generated SEO data exceeds the maximum length of {maxlength} characters!"); }
            throw new ArgumentOutOfRangeException($"Oluşturulan SEO verisi {maxlength} karakterlik maksimum uzunluğu aşıyor!");
        }
        /// <summary> IQueryable koleksiyonunu asenkron olarak sayfalanmış bir listeye dönüştürür. </summary>
        /// <typeparam name="T">Sorgu sonucundaki öğelerin tipi.</typeparam>
        /// <param name="source">Sayfalanacak IQueryable veri kaynağı.</param>
        /// <param name="page">İstenen sayfa numarası. (1 tabanlı)</param>
        /// <param name="size">Sayfa başına öğe sayısı.</param>
        /// <param name="loadinfo">Sayfalama bilgilerinin (toplam sayfa, toplam öğe sayısı vb.) yüklenip yüklenmeyeceğini belirtir. Varsayılan değer: <see langword="true"/>.</param>
        /// <param name="cancellationToken">Asenkron işlemi iptal etmek için kullanılan token.</param>
        public static async Task<Paginate<T>> ToPagedListAsync<T>(this IQueryable<T> source, int page, int size, bool loadinfo = true, CancellationToken cancellationToken = default)
        {
            if (source == null) { return new(); }
            PagingInfo? p = null;
            if (loadinfo)
            {
                var totalCount = await source.CountAsync(cancellationToken);
                var totalPage = Convert.ToInt32(Math.Ceiling(totalCount / Convert.ToDouble(size)));
                p = new(totalCount, totalPage, page);
            }
            var items = await source.Paginate(page, size).ToArrayAsync(cancellationToken);
            return new(page, size, items, p);
        }
    }
}