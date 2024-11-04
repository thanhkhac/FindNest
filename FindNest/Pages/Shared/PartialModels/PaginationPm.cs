namespace FindNest.Pages.Shared.PartialModels
{
    public class PaginationPm
    {
        public string? ParamName { get; set; }
        public string? FormName { get; set; }
        public int PageCount { get; set; }
        public int CurrentPage { get; set; }
        
        public int TotalCount { get; set; }
    }
}