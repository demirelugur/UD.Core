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
        private int _Pagenumber;
        private int _Size;
        private T[] _Items;
        private PagingInfo? _Info;
        public int pagenumber { get { return _Pagenumber; } set { _Pagenumber = value; } }
        public int size { get { return _Size; } set { _Size = value; } }
        public T[] items { get { return _Items; } set { _Items = value ?? Array.Empty<T>(); } }
        public PagingInfo? info { get { return _Info; } set { _Info = value; } }
        public Paginate() : this(default, default, default, default) { }
        public Paginate(int pagenumber, int size, T[] items, PagingInfo? info)
        {
            this.pagenumber = pagenumber;
            this.size = size;
            this.items = items;
            this.info = info;
        }
    }
}