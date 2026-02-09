namespace UD.Core.Helper.Paging
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using UD.Core.Extensions;
    public interface IPaginate<T>
    {
        int Page { get; }
        int Size { get; }
        T[] Items { get; }
        PagingInfo? Info { get; }
    }
    public class Paginate<T> : IPaginate<T>
    {
        public Paginate() : this(default, default, default, default) { }
        public Paginate(int page, int size, T[] items, PagingInfo? info)
        {
            this.Page = page;
            this.Size = size;
            this.Items = items ?? Array.Empty<T>();
            this.Info = info;
        }
        public int Page { get; set; }
        public int Size { get; set; }
        public T[] Items { get; set; }
        public PagingInfo? Info { get; set; }
        public static async Task<Paginate<T>> ToPagedListAsync(IQueryable<T> source, int page, int size, CancellationToken cancellationToken = default)
        {
            if (source == null) { return new(); }
            var totalCount = await source.CountAsync(cancellationToken);
            var totalPage = Convert.ToInt32(Math.Ceiling(totalCount / Convert.ToDouble(size)));
            var items = await source.Paginate(page, size).ToArrayAsync(cancellationToken);
            return new(page, size, items, new(totalCount, totalPage, page));
        }
    }
}