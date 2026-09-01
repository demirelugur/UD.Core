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
        public PagingInfo(int totalCount, int totalPage, int page) : this(totalCount, totalPage, page < totalPage, page > 1) { }
        public PagingInfo(int totalCount, int totalPage, bool hasNext, bool hasPrevious)
        {
            this.TotalCount = totalCount;
            this.TotalPage = totalPage;
            this.HasNext = hasNext;
            this.HasPrevious = hasPrevious;
        }
    }
}