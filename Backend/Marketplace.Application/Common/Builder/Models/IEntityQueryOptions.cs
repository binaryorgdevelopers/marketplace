﻿namespace Marketplace.Application.Common.Builder.Models;

/// <summary>
/// Defines properties for queryable entities source query options
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface IEntityQueryOptions<TEntity> : IQueryOptions<TEntity> where TEntity : class, IEntity
{
    /// <summary>
    /// Requested include model options
    /// </summary>
    IncludeOptions<TEntity>? IncludeOptions { get; set; }
}