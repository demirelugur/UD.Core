namespace UD.Core.Helper.Paging
{
    using System;
    /// <summary> Sayfalanmış veri koleksiyonunu temsil eden arayüz.</summary>
    public interface IPaginate<T>
    {
        int Page { get; }
        int Size { get; }
        T[] Items { get; }
        PagingInfo? Info { get; }
    }
    /// <summary> Sayfalanmış veri koleksiyonunun somut implementasyonu. </summary>
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
    }
}