namespace UD.Core.Pages
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
        public int PageNumber { get; set; }
        [UDRangePositiveInt32]
        [DefaultValue(20)]
        public int Size { get; set; }
        public string? Ordering { get => _ordering; set => _ordering = value.ParseOrDefault<string>(); }
        public SearchAndPaginateDto() : this(default, default, default) { }
        public SearchAndPaginateDto(int pageNumber, int size, string? ordering)
        {
            this.PageNumber = Math.Max(0, pageNumber);
            this.Size = size;
            this.Ordering = ordering;
        }
        public virtual Task<Paginate<T>> ToPagedListAsync<T>(IQueryable<T> source, bool loadInfo, CancellationToken cancellationToken) => source.ToPagedListAsync(this.PageNumber, this.Size, this.Ordering, loadInfo, cancellationToken);
    }
}