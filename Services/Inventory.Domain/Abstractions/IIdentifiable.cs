namespace Inventory.Domain.Abstractions;

/// <summary>
///     Common Interface for Database Entities
/// </summary>
public interface IIdentifiable
{
    /// <summary>
    ///     Gets or sets Primary Key
    /// </summary>
    Guid Id { get; set; }
}