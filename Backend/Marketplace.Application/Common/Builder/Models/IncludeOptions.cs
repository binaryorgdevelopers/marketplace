﻿namespace Marketplace.Application.Common.Builder.Models;

/// <summary>
/// Represents including options
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public class IncludeOptions<TEntity> where TEntity : class, IEntity
{
    public IncludeOptions()
    {
        IncludeModels = new List<string>();
    }

    public IEnumerable<string> IncludeModels { get; set; }
}