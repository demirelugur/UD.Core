namespace UD.Core.Helper.Paging
{
    using System.ComponentModel;
    using UD.Core.Attributes.DataAnnotations;
    using UD.Core.Extensions;
    using UD.Core.Extensions.Common;

    public interface ISearchAndPaginateDto
    {
        int pagenumber { get; set; }
        int size { get; set; }
        string? sorting { get; set; }
        Task<Paginate<T>> ToPagedList<T>(IQueryable<T> source, bool loadInfo, CancellationToken cancellationToken);
    }
    [Serializable]
    public class SearchAndPaginateDto : ISearchAndPaginateDto
    {
        private string? _Sorting;
        [UDRangePositiveInt32]
        [DefaultValue(1)]
        public int pagenumber { get; set; }
        [UDRangePositiveInt32]
        [DefaultValue(20)]
        public int size { get; set; }
        public string? sorting { get { return _Sorting; } set { _Sorting = value.ParseOrDefault<string>(); } }
        public SearchAndPaginateDto() : this(default, default, default) { }
        public SearchAndPaginateDto(int pagenumber, int size, string? sorting)
        {
            this.pagenumber = pagenumber;
            this.size = size;
            this.sorting = sorting;
        }
        public virtual Task<Paginate<T>> ToPagedList<T>(IQueryable<T> source, bool loadInfo, CancellationToken cancellationToken) => source.ToPagedList(this.pagenumber, this.size, this.sorting, loadInfo, cancellationToken);
    }
}