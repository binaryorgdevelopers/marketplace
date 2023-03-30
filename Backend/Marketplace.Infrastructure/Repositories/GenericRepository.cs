using System.Linq.Expressions;
using Marketplace.Domain.Abstractions;
using Marketplace.Domain.Abstractions.Repositories;
using Marketplace.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Infrastructure.Repositories;

/// <summary>
/// Provides methods for generic entity repository base to manipulate entities
/// </summary>
/// <typeparam name="TEntity">Entity type</typeparam>
public class GenericRepository<TEntity> : IGenericRepository<TEntity>
    where TEntity : class, IIdentifiable, new()
{
    private readonly DataContext _dataContext;

    public GenericRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate)
        => await _dataContext
            .Set<TEntity>()
            .AsNoTracking()
            .FirstOrDefaultAsync(predicate);


    public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate)
        => _dataContext.Set<TEntity>().Where(predicate);

    public TSelect? GetWithSelect<TSelect>(
        Func<TSelect, bool> predicate,
        Expression<Func<TEntity, TSelect>> select,
        params Expression<Func<TEntity, object>>[] includes) where TSelect : new()

    {
        var entity = _dataContext.Set<TEntity>()
            .AsNoTracking()
            .AsQueryable();
        var included = includes
            .Aggregate(entity, (query, path) =>
                query.Include(path));
        return included
            .Select(select)
            .FirstOrDefault(predicate);
    }

    public async Task AddAsync(TEntity entity)
    {
        await _dataContext.Set<TEntity>().AddAsync(entity);
        await _dataContext.SaveChangesAsync();
    }

    public void Update(TEntity entity)
    {
        _dataContext.Set<TEntity>().Update(entity);
        _dataContext.SaveChanges();
    }

    public async Task DeleteAsync(Guid id)
        => _dataContext
            .Set<TEntity>()
            .Remove((await GetAsync(c => c.Id == id))!);

    public Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
        => _dataContext.Set<TEntity>().AnyAsync(predicate);

    public async Task<IEnumerable<TEntity>> GetAllAsync() => await _dataContext.Set<TEntity>().ToListAsync();

    public async Task SaveChangesAsync() => await _dataContext.SaveChangesAsync();

    public async Task AddWithoutSaveAsync(TEntity entity) => await _dataContext.AddAsync(entity);
}