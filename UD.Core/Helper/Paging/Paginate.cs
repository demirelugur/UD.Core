namespace UD.Core.Helper.Paging
{
    using System;
    public interface IPaginate<T>
    {
        int pagenumber { get; set; }
        int size { get; set; }
        T[] items { get; set; }
        PagingInfo? info { get; set; }
    }
    public class Paginate<T> : IPaginate<T>
    {
        private T[] _Items;
        public int pagenumber { get; set; }
        public int size { get; set; }
        public T[] items { get { return _Items; } set { _Items = value ?? []; } }
        public PagingInfo? info { get; set; }
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