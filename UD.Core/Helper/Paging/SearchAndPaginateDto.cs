namespace UD.Core.Helper.Paging
{
    using UD.Core.Attributes.DataAnnotations;
    public interface ISearchAndPaginateDto
    {
        int pagenumber { get; set; }
        int size { get; set; }
        string? sorting { get; set; }
    }
    public class SearchAndPaginateDto : ISearchAndPaginateDto
    {
        [Validation_RangePositiveInt32]
        public int pagenumber { get; set; } = 1;
        [Validation_RangePositiveInt32]
        public int size { get; set; } = 20;
        public string? sorting { get; set; }
        public SearchAndPaginateDto() : this(default, default, default) { }
        public SearchAndPaginateDto(int pagenumber, int size, string? sorting)
        {
            this.pagenumber = pagenumber;
            this.size = size;
            this.sorting = sorting;
        }
    }
}