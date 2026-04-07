namespace UD.Core.Helper.Paging
{
    public interface IPaginate<T>
    {
        int pageNumber { get; set; }
        int size { get; set; }
        T[] items { get; set; }
        PagingInfo? info { get; set; }
    }
    [Serializable]
    public class Paginate<T> : IPaginate<T>
    {
        private T[] _Items;
        public int pageNumber { get; set; }
        public int size { get; set; }
        public T[] items { get { return _Items; } set { _Items = value ?? []; } }
        public PagingInfo? info { get; set; }
        public Paginate() : this(default, default, default, default) { }
        public Paginate(int pageNumber, int size, T[] items, PagingInfo? info)
        {
            this.pageNumber = pageNumber;
            this.size = size;
            this.items = items;
            this.info = info;
        }
    }
}