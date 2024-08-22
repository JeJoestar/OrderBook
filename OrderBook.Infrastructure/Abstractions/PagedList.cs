public sealed class PagedList<TData, TKey>(
    IEnumerable<TData> items,
    TKey? pageNumber,
    int pageSize)
{
    public IEnumerable<TData> Items { get; } = items;

    public TKey? PageNumber { get; } = pageNumber;

    public int PageSize { get; } = pageSize;

    public bool HasNextPage => PageNumber is not null;
}