namespace UD.Core.Helper.Paging
{
    public sealed class PagingInfo
    {
        private int _Totalcount;
        private int _Totalpage;
        private bool _Hasnext;
        private bool _Hasprevious;
        public int totalcount { get { return _Totalcount; } set { _Totalcount = value; } }
        public int totalpage { get { return _Totalpage; } set { _Totalpage = value; } }
        public bool hasnext { get { return _Hasnext; } set { _Hasnext = value; } }
        public bool hasprevious { get { return _Hasprevious; } set { _Hasprevious = value; } }
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