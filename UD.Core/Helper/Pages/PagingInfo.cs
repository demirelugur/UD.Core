namespace UD.Core.Helper.Pages
{
    [Serializable]
    public sealed class PagingInfo
    {
        public int TotalCount { get; set; }
        public int TotalPage { get; set; }
        public bool HasNext { get; set; }
        public bool HasPrevious { get; set; }
        public PagingInfo() : this(default, default, default, default) { }
        public PagingInfo(int TotalCount, int TotalPage, int page) : this(TotalCount, TotalPage, page < TotalPage, page > 1) { }
        public PagingInfo(int TotalCount, int TotalPage, bool HasNext, bool HasPrevious)
        {
            this.TotalCount = TotalCount;
            this.TotalPage = TotalPage;
            this.HasNext = HasNext;
            this.HasPrevious = HasPrevious;
        }
    }
}