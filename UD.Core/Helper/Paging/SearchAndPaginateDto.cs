namespace UD.Core.Helper.Paging
{
    using System.ComponentModel;
    using UD.Core.Attributes.DataAnnotations;
    using UD.Core.Extensions;
    public interface ISearchAndPaginateDto
    {
        int pagenumber { get; set; }
        int size { get; set; }
        string? sorting { get; set; }
        Task<Paginate<T>> ToPagedListAsync<T>(IQueryable<T> source, bool loadinfo, CancellationToken cancellationToken);
    }
    [Serializable]
    public class SearchAndPaginateDto : ISearchAndPaginateDto
    {
        private int _PageNumber;
        private int _Size;
        private string? _Sorting;
        [Validation_RangePositiveInt32]
        [DefaultValue(1)]
        public int pagenumber { get { return _PageNumber; } set { _PageNumber = value; } }
        [Validation_RangePositiveInt32]
        [DefaultValue(20)]
        public int size { get { return _Size; } set { _Size = value; } }
        [DefaultValue(null)]
        public string? sorting { get { return _Sorting; } set { _Sorting = value.ParseOrDefault<string>(); } }
        public SearchAndPaginateDto() : this(default, default, default) { }
        public SearchAndPaginateDto(int pagenumber, int size, string? sorting)
        {
            this.pagenumber = pagenumber;
            this.size = size;
            this.sorting = sorting;
        }
        public virtual Task<Paginate<T>> ToPagedListAsync<T>(IQueryable<T> source, bool loadinfo, CancellationToken cancellationToken) => source.ToPagedListAsync(this.pagenumber, this.size, this.sorting, loadinfo, cancellationToken);
    }
}