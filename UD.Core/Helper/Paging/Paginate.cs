namespace UD.Core.Helper.Paging
{
    using System;
    /// <summary> Sayfalanmış veri koleksiyonunu temsil eden arayüz.</summary>
    public interface IPaginate<T>
    {
        int page { get; }
        int size { get; }
        T[] items { get; }
        PagingInfo? info { get; }
    }
    /// <summary> Sayfalanmış veri koleksiyonunun somut implementasyonu. </summary>
    public class Paginate<T> : IPaginate<T>
    {
        public int page { get; set; }
        public int size { get; set; }
        public T[] items { get; set; }
        public PagingInfo? info { get; set; }
        public Paginate() : this(default, default, default, default) { }
        public Paginate(int page, int size, T[] items, PagingInfo? info)
        {
            this.page = page;
            this.size = size;
            this.items = items ?? Array.Empty<T>();
            this.info = info;
        }
    }
}