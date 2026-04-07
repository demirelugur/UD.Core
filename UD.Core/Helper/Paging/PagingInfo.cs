namespace UD.Core.Helper.Paging
{
    [Serializable]
    public sealed class PagingInfo
    {
        public int totalCount { get; set; }
        public int totalPage { get; set; }
        public bool hasNext { get; set; }
        public bool hasPrevious { get; set; }
        public PagingInfo() : this(default, default, default, default) { }
        public PagingInfo(int totalCount, int totalPage, int page) : this(totalCount, totalPage, page < totalPage, page > 1) { }
        public PagingInfo(int totalCount, int totalPage, bool hasNext, bool hasPrevious)
        {
            this.totalCount = totalCount;
            this.totalPage = totalPage;
            this.hasNext = hasNext;
            this.hasPrevious = hasPrevious;
        }
    }
}