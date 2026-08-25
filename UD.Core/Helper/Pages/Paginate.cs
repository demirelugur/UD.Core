namespace UD.Core.Helper.Pages
{
    public interface IPaginate<T>
    {
        int PageNumber { get; set; }
        int Size { get; set; }
        T[] Items { get; set; }
        PagingInfo? Info { get; set; }
    }
    [Serializable]
    public class Paginate<T> : IPaginate<T>
    {
        public int PageNumber { get; set; }
        public int Size { get; set; }
        public T[] Items { get; set; } = [];
        public PagingInfo? Info { get; set; }
        public Paginate() : this(default, default, default, default) { }
        public Paginate(int PageNumber, int Size, T[] Items, PagingInfo? Info)
        {
            this.PageNumber = PageNumber;
            this.Size = Size;
            this.Items = Items ?? [];
            this.Info = Info;
        }
    }
}