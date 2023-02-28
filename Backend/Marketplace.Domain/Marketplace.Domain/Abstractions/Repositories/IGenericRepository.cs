using System.Linq.Expressions;

namespace Marketplace.Domain.Abstractions.Repositories;

public interface IGenericRepository<TEntity>
{
    Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate);
    Task<IQueryable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
    TEntity? Get(Expression<Func<TEntity, bool>> predicate);
    Task AddAsync(TEntity entity);
    void Update(TEntity entity);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
    IQueryable<TEntity> GetAll();
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task SaveChangesAsync();
    Task AddWithoutSaveAsync(TEntity entity);

    IEnumerable<TSelect> GetWithInclude<TProperty, TSelect>(Expression<Func<TEntity, TProperty>> include,
        Expression<Func<TEntity, TSelect>> select);

    TSelect? GetSingleWithInclude<TProperty, TSelect>(
        Expression<Func<TEntity, TProperty>> include,
        Expression<Func<TEntity, TSelect>> select,
        Expression<Func<TSelect?, bool>> predicate);
}