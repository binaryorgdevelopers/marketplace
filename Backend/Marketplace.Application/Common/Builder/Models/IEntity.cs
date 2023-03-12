namespace Marketplace.Application.Common.Builder.Models;

/// <summary>
/// Common Interface for Database Entities 
/// </summary>
public interface IEntity
{
    /// <summary>
    /// Gets or sets Primary Key 
    /// </summary>
    long Id { get; set; }
}