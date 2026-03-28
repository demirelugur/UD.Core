namespace UD.Core.Helper.Paging
{
    [Serializable]
    public sealed class PagingInfo
    {
        public int totalcount { get; set; }
        public int totalpage { get; set; }
        public bool hasnext { get; set; }
        public bool hasprevious { get; set; }
        public PagingInfo() : this(default, default, default, default) { }
        public PagingInfo(int totalcount, int totalpage, int page) : this(totalcount, totalpage, page < totalpage, page > 1) { }
        public PagingInfo(int totalcount, int totalpage, bool hasnext, bool hasprevious)
        {
            this.totalcount = totalcount;
            this.totalpage = totalpage;
            this.hasnext = hasnext;
            this.hasprevious = hasprevious;
        }
    }
}