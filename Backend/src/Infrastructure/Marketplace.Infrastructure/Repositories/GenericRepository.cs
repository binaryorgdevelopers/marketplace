using System.Linq.Expressions;
using Marketplace.Domain.Abstractions;
using Marketplace.Domain.Repositories;
using Marketplace.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Infrastructure.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity>
    where TEntity : class, IIdentifiable, new()
{
    private readonly DataContext _authContext;

    public GenericRepository(DataContext authContext)
    {
        _authContext = authContext;
    }


    public async Task<TEntity?> GetAsync(Guid id)
        => await GetAsync(e => e.Id == id);

    public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate)
        => await _authContext.Set<TEntity>().FirstOrDefaultAsync(predicate);

    public async Task<IQueryable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
    {
        await Task.CompletedTask;
        return _authContext.Set<TEntity>().Where(predicate).AsQueryable();
    }

    public async Task AddAsync(TEntity entity)
    {
        await _authContext.Set<TEntity>().AddAsync(entity);
        await _authContext.SaveChangesAsync();
    }

    public void Update(TEntity entity)
    {
        _authContext.Set<TEntity>().Update(entity);
        _authContext.SaveChanges();
    }

    public async Task DeleteAsync(Guid id)
        => _authContext.Set<TEntity>().Remove(await GetAsync(id));

    public Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
        => _authContext.Set<TEntity>().AnyAsync(predicate);
}
