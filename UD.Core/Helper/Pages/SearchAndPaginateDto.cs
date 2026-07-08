namespace UD.Core.Helper.Pages
{
    using System.ComponentModel;
    using UD.Core.Attributes.DataAnnotations;
    using UD.Core.Extensions;
    public interface ISearchAndPaginateDto
    {
        int pageNumber { get; set; }
        int size { get; set; }
        string? ordering { get; set; }
        Task<Paginate<T>> ToPagedList<T>(IQueryable<T> source, bool loadInfo, CancellationToken cancellationToken);
    }
    [Serializable]
    public class SearchAndPaginateDto : ISearchAndPaginateDto
    {
        private string? _Ordering;
        [UDRangePositiveInt32]
        [DefaultValue(1)]
        public int pageNumber { get; set; }
        [UDRangePositiveInt32]
        [DefaultValue(20)]
        public int size { get; set; }
        public string? ordering { get { return _Ordering; } set { _Ordering = value.ParseOrDefault<string>(); } }
        public SearchAndPaginateDto() : this(default, default, default) { }
        public SearchAndPaginateDto(int pageNumber, int size, string? ordering)
        {
            this.pageNumber = pageNumber;
            this.size = size;
            this.ordering = ordering;
        }
        public virtual Task<Paginate<T>> ToPagedList<T>(IQueryable<T> source, bool loadInfo, CancellationToken cancellationToken) => source.ToPagedList(this.pageNumber, this.size, this.ordering, loadInfo, cancellationToken);
    }
}