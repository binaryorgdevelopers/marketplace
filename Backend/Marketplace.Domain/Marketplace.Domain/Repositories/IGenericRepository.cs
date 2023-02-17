using System.Linq.Expressions;

namespace Marketplace.Domain.Repositories;

public interface IGenericRepository<TEntity>
{
    Task<TEntity?> GetAsync(Guid id);
    Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate);
    Task<IQueryable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
    Task AddAsync(TEntity entity);
    void Update(TEntity entity);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
}