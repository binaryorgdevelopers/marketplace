﻿using System.Linq.Expressions;

namespace Marketplace.Domain.Abstractions.Repositories;

public interface IGenericRepository<TEntity>
{
    /// <summary>
    /// Returns TEntity with given predicate
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    /// Adds Entity to Set<TEntity> table
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task AddAsync(TEntity entity);

    /// <summary>
    /// Updates TEntity
    /// </summary>
    /// <param name="entity"></param>
    void Update(TEntity entity);

    /// <summary>
    /// Deleted TEntity with given Guid Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// Checks whether item exists or not with matching predicate
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    /// Async version of GetAll
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<TEntity>> GetAllAsync();

    /// <summary>
    /// Closes transactions and saved changed states to Database
    /// </summary>
    /// <returns></returns>
    Task SaveChangesAsync();

    /// <summary>
    /// Adds TEntity to Database without saving
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task AddWithoutSaveAsync(TEntity entity);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="includes">Predicate for Include</param>
    /// <param name="select">Custom mapping data</param>
    /// <param name="predicate">Predicate for filtering</param>
    /// <typeparam name="TInclude">Item to be added with Include</typeparam>
    /// <typeparam name="TSelect">Entity for Selecting custom mapped data</typeparam>
    /// <returns></returns>
    TSelect? GetWithSelect<TInclude, TSelect>(Expression<Func<TEntity, TInclude>>[] includes,
        Func<TSelect, bool> predicate,
        Expression<Func<TEntity, TSelect>> select) where TSelect : new();
}