using System.Linq.Expressions;
using Marketplace.Domain.Entities;

namespace Marketplace.Domain.Abstractions.Repositories;

public interface IGenericRepository<TEntity>
{
    Task<TEntity?> GetAsync(Guid id);
    Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate);
    Task<IQueryable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
    TEntity? Get(Expression<Func<TEntity, bool>> predicate);
    Task AddAsync(TEntity entity);
    void Update(TEntity entity);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
    IEnumerable<TEntity> GetAll<TProperty>(Expression<Func<TEntity, TProperty>> predicate);

    IEnumerable<TSelect> GetWithInclude<TProperty, TSelect>(Expression<Func<TEntity, TProperty>> include,
        Expression<Func<TEntity, TSelect>> select);
}