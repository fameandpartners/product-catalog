namespace Fame.Service.DTO
{
    public abstract class PagedArgs
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public abstract class SortablePagedArgs : PagedArgs
    {
        public string SortField { get; set; }
    }
}
