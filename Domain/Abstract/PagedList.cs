namespace Domain.Abstract;

public class PagedList<T>
    where T : class
{
    public int TotalCount { get; set; }

    public int PageSize { get; set; }

    public int CurrentPageNumber { get; set; }

    public int TotalPages { get; set; }

    public bool HasPreviousPage { get; set; }

    public bool HasNextPage { get; set; }

    public IEnumerable<T> Items { get; set; }

    public PagedList(
        int totalCount,
        IEnumerable<T> items,
        int currentPageNumber,
        int pageSize)
    {
        TotalCount = totalCount;
        Items = items;
        CurrentPageNumber = currentPageNumber;
        PageSize = pageSize;

        TotalPages = (int)Math
            .Ceiling((double)TotalCount / (double)PageSize);

        HasPreviousPage = CurrentPageNumber > 1;
        HasNextPage = CurrentPageNumber < TotalPages;
    }
}
