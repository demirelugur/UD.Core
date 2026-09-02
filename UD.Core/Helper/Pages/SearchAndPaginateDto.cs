namespace UD.Core.Helper.Pages
{
    using System.ComponentModel;
    using UD.Core.Attributes.DataAnnotations;
    using UD.Core.Extensions;
    public interface ISearchAndPaginateDto
    {
        int PageNumber { get; set; }
        int Size { get; set; }
        string? Ordering { get; set; }
        Task<Paginate<T>> ToPagedListAsync<T>(IQueryable<T> source, bool loadInfo, CancellationToken cancellationToken);
    }
    [Serializable]
    public class SearchAndPaginateDto : ISearchAndPaginateDto
    {
        private string? _ordering;
        [UDRangePositiveInt32]
        [DefaultValue(1)]
        public int PageNumber { get; set; }
        [UDRangePositiveInt32]
        [DefaultValue(20)]
        public int Size { get; set; }
        public string? Ordering { get { return _ordering; } set { _ordering = value.ParseOrDefault<string>(); } }
        public SearchAndPaginateDto() : this(default, default, default) { }
        public SearchAndPaginateDto(int pageNumber, int size, string? ordering)
        {
            this.PageNumber = pageNumber;
            this.Size = size;
            this.Ordering = ordering;
        }
        public virtual Task<Paginate<T>> ToPagedListAsync<T>(IQueryable<T> source, bool loadInfo, CancellationToken cancellationToken) => source.ToPagedListAsync(this.PageNumber, this.Size, this.Ordering, loadInfo, cancellationToken);
    }
}