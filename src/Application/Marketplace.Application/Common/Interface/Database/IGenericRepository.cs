﻿using System.Linq.Expressions;

namespace Marketplace.Application.Common.Interface.Database;

public interface IGenericRepository<TEntity>
{
    Task<TEntity?> GetAsync(Guid id);
    Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate);
    Task<IQueryable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);

    // Task<PagedResult<TEntity>> BrowseAsync<TQuery>(Expression<Func<TEntity, bool>> predicate,
    //     TQuery query) where TQuery : PagedQueryBase;

    Task AddAsync(TEntity entity);
    void Update(TEntity entity);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
}