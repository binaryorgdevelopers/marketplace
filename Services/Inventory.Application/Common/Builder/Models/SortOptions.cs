namespace Marketplace.Application.Common.Builder.Models;

public class SortOptions
{
    /// <summary>
    /// Sort Field
    /// </summary>
    public string SortField { get; set; } = null!;

    /// <summary>
    /// Indicates whether to sort ascending
    /// </summary>
    public string SortAscending { get; set; }
}