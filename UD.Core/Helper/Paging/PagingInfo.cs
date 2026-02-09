namespace UD.Core.Helper.Paging
{
    /// <summary> Sayfalama işlemine ait toplam sayı, sayfa bilgisi ve gezinme durumlarını içeren sınıf. </summary>
    public sealed class PagingInfo
    {
        public int totalcount { get; set; }
        public int totalpage { get; set; }
        public bool hasnext { get; set; }
        public bool hasprevious { get; set; }
        public PagingInfo() : this(default, default, default) { }
        public PagingInfo(int totalcount, int totalpage, int page)
        {
            this.totalcount = totalcount;
            this.totalpage = totalpage;
            this.hasnext = page < totalpage;
            this.hasprevious = page > 1;
        }
    }
}