namespace PetFamily.Core.Models;

public class PagedList<T>
{
    public  IReadOnlyList<T> Items { get; init; }
    public int Page { get; init; }
    public int PageSize { get; init; }
    public int TotalCount { get; init; }
    public bool HasPreviousPage => Page > 1;
    public bool HasNextPage => Page * PageSize < TotalCount;
    
    public  PagedList<T> AfterLoad(Action<T> processItem)
    {
        var updatedItems = Items.Select(item =>
        {
            processItem(item);
            return item;
        }).ToList();

        return new PagedList<T>
        {
            Items = updatedItems,
            Page = Page,
            PageSize = PageSize,
            TotalCount = TotalCount
        };
    }
}