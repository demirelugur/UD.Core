namespace UD.Core.Helper.Paging
{
    using System;
    /// <summary> Sayfalanmış veri koleksiyonunu temsil eden arayüz.</summary>
    public interface IPaginate<T>
    {
        int pagenumber { get; set; }
        int size { get; set; }
        T[] items { get; set; }
        PagingInfo? info { get; set; }
    }
    /// <summary> Sayfalanmış veri koleksiyonunun somut implementasyonu. </summary>
    public class Paginate<T> : IPaginate<T>
    {
        public int pagenumber { get; set; }
        public int size { get; set; }
        public T[] items { get; set; }
        public PagingInfo? info { get; set; }
        public Paginate() : this(default, default, default, default) { }
        public Paginate(int pagenumber, int size, T[] items, PagingInfo? info)
        {
            this.pagenumber = pagenumber;
            this.size = size;
            this.items = items ?? Array.Empty<T>();
            this.info = info;
        }
    }
}