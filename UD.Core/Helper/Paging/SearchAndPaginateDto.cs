namespace UD.Core.Helper.Paging
{
    using UD.Core.Attributes.DataAnnotations;
    using UD.Core.Extensions;
    public interface ISearchAndPaginateDto
    {
        int pagenumber { get; set; }
        int size { get; set; }
        string? sorting { get; set; }
    }
    [Serializable]
    public class SearchAndPaginateDto : ISearchAndPaginateDto
    {
        private int _PageNumber = 1;
        private int _Size = 20;
        private string? _Sorting;
        [Validation_RangePositiveInt32]
        public int pagenumber { get { return _PageNumber; } set { _PageNumber = value; } }
        [Validation_RangePositiveInt32]
        public int size { get { return _Size; } set { _Size = value; } }
        public string? sorting { get { return _Sorting; } set { _Sorting = value.ParseOrDefault<string>(); } }
        public SearchAndPaginateDto() : this(default, default, default) { }
        public SearchAndPaginateDto(int pagenumber, int size, string? sorting)
        {
            this.pagenumber = pagenumber;
            this.size = size;
            this.sorting = sorting;
        }
    }
}