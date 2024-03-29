﻿using System.Linq.Expressions;

namespace Inventory.Domain.Abstractions.Repositories;

public interface IGenericRepository<TEntity>
{
    /// <summary>
    ///     Returns TEntity with given predicate
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Adds Entity to Set<TEntity> table
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Performs Bulk update to table
    /// </summary>
    /// <param name="entities">Entities to add</param>
    /// <param name="cancellationToken">Optional parameter for cancelling operation</param>
    /// <returns></returns>
    Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = new());

    /// <summary>
    ///     Gets entities matching the predicate
    /// </summary>
    /// <param name="expression"></param>
    /// <param name="predicate"></param>
    /// <returns>Entities query</returns>
    IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    ///     Updates TEntity
    /// </summary>
    /// <param name="entity"></param>
    void Update(TEntity entity);

    /// <summary>
    ///     Deleted TEntity with given Guid Id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = new());

    /// <summary>
    ///     Checks whether item exists or not with matching predicate
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = new());

    /// <summary>
    ///     Async version of GetAll
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = new());

    /// <summary>
    ///     Closes transactions and saved changed states to Database
    /// </summary>
    /// <returns></returns>
    Task SaveChangesAsync(CancellationToken cancellationToken = new());

    /// <summary>
    ///     Adds TEntity to Database without saving
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task AddWithoutSaveAsync(TEntity entity, CancellationToken cancellationToken = new());

    /// <summary>
    ///     Retrieves single entity with selectable relations from Database with given predicate
    /// </summary>
    /// <param name="predicate">Predicate for searching</param>
    /// <param name="select"></param>
    /// <param name="includes"></param>
    /// <typeparam name="TSelect">Entity type for mapping object</typeparam>
    /// <returns></returns>
    TSelect? GetWithSelect<TSelect>(
        Func<TSelect, bool> predicate,
        Expression<Func<TEntity, TSelect>> select,
        params Expression<Func<TEntity, object>>[] includes) where TSelect : new();
}