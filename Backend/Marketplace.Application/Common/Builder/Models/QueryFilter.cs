namespace Marketplace.Application.Common.Builder.Models;

/// <summary>
/// Represents queryable source query options
/// </summary>
public class QueryFilter
{
    /// <summary>
    /// Field key name
    /// </summary>
    public string Key { get; set; } = null!;

    /// <summary>
    /// Filtering Value
    /// </summary>
    public string Value { get; set; } = null!;
}