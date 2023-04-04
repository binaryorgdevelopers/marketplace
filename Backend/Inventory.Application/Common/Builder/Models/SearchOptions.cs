namespace Marketplace.Application.Common.Builder.Models;

public class SearchOptions<TSource> where TSource : class
{
    /// <summary>
    /// Search Keyword
    /// </summary>
    public string Keyword { get; set; } = null!;

    /// <summary>
    /// Determines whether to search from direct children
    /// </summary>
    public bool IncludeChildren { get; set; }
}