namespace Marketplace.Application.Common.Builder.Models;

/// <summary>
/// Represents queryable source query options
/// </summary>
/// <typeparam name="TSource"></typeparam>
public class QueryOptions<TSource> : IQueryOptions<TSource> where TSource : class
{
    public SearchOptions<TSource>? SearchOptions { get; set; }

    public FilterOptions<TSource>? FilterOptions { get; set; }

    public SortOptions? SortOptions { get; set; }

    public PaginationOptions PaginationOptions { get; set; } = null!;
}