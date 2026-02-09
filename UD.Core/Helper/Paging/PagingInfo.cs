namespace UD.Core.Helper.Paging
{
    public sealed class PagingInfo
    {
        public int TotalCount { get; set; }
        public int TotalPage { get; set; }
        public bool HasNext { get; set; }
        public bool HasPrevious { get; set; }
        public PagingInfo() : this(default, default, default) { }
        public PagingInfo(int totalCount, int totalPage, int page)
        {
            this.TotalCount = totalCount;
            this.TotalPage = totalPage;
            this.HasNext = page < totalPage;
            this.HasPrevious = page > 1;
        }
    }
}