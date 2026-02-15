namespace UD.Core.Helper.Paging
{
    using System.Linq.Dynamic.Core;
    using UD.Core.Attributes.DataAnnotations;
    using UD.Core.Extensions;
    public interface ISearchAndPaginateDto
    {
        int pagenumber { get; set; }
        int size { get; set; }
        string? sorting { get; set; }
        IQueryable<TEntity> Paginate<TEntity>(IQueryable<TEntity> query);
    }
    public class SearchAndPaginateDto : ISearchAndPaginateDto
    {
        [Validation_RangePositiveInt32]
        public int pagenumber { get; set; }
        [Validation_RangePositiveInt32]
        public int size { get; set; }
        public string? sorting { get; set; }
        public SearchAndPaginateDto() : this(default, default, default) { }
        public SearchAndPaginateDto(int pagenumber, int size, string? sorting)
        {
            this.pagenumber = pagenumber;
            this.size = size;
            this.sorting = sorting;
        }
        public IQueryable<TEntity> Paginate<TEntity>(IQueryable<TEntity> query)
        {
            if (!this.sorting.IsNullOrEmpty())
            {
                try { query = query.OrderBy(this.sorting); }
                catch (Exception ex) { throw new InvalidOperationException($"Sorting failed: {this.sorting}", ex); }
            }
            return query.Paginate(this.pagenumber, this.size);
        }
    }
}