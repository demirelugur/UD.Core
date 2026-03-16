namespace UD.Core.Helper.Paging
{
    [Serializable]
    public sealed class PagingInfo
    {
        public long totalcount { get; set; }
        public long totalpage { get; set; }
        public bool hasnext { get; set; }
        public bool hasprevious { get; set; }
        public PagingInfo() : this(default, default, default, default) { }
        public PagingInfo(long totalcount, long totalpage, int page) : this(totalcount, totalpage, page < totalpage, page > 1) { }
        public PagingInfo(long totalcount, long totalpage, bool hasnext, bool hasprevious)
        {
            this.totalcount = totalcount;
            this.totalpage = totalpage;
            this.hasnext = hasnext;
            this.hasprevious = hasprevious;
        }
    }
}