namespace UD.Core.Helper.Paging
{
    public sealed class PagingInfo
    {
        private long _Totalcount;
        private long _Totalpage;
        private bool _Hasnext;
        private bool _Hasprevious;
        public long totalcount { get { return _Totalcount; } set { _Totalcount = value; } }
        public long totalpage { get { return _Totalpage; } set { _Totalpage = value; } }
        public bool hasnext { get { return _Hasnext; } set { _Hasnext = value; } }
        public bool hasprevious { get { return _Hasprevious; } set { _Hasprevious = value; } }
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