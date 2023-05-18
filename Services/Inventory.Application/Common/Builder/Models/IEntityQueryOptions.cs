using Inventory.Domain.Abstractions;

namespace Marketplace.Application.Common.Builder.Models;

/// <summary>
/// Defines properties for queryable entities source query options
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface IEntityQueryOptions<TEntity> : IQueryOptions<TEntity> where TEntity : class, IIdentifiable
{
    /// <summary>
    /// Requested include model options
    /// </summary>
    IncludeOptions<TEntity>? IncludeOptions { get; set; }
}