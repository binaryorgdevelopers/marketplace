using System.Linq.Expressions;
using Inventory.Domain.Abstractions;
using Inventory.Domain.Abstractions.Repositories;
using Marketplace.Infrastructure.Persistence;
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

    public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
        => await _dataContext
            .Set<TEntity>()
            .AsNoTracking()
            .FirstOrDefaultAsync(predicate, cancellationToken: cancellationToken);


    public async Task AddRangeAsync(IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = new())
        => await _dataContext.Set<TEntity>()
            .AddRangeAsync(entities, cancellationToken);

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

    // public TEntity? GetWithInclude(
    //     Func<TEntity, bool> predicate,
    //     params Expression<Func<TEntity,object>>[] includes)
    // {
    //     var entity = _dataContext.Set<TEntity>()
    //         .AsNoTracking().AsEnumerable()
    //         .Where(predicate);
    //     var included = includes
    //         .Aggregate(_dataContext.Set<TEntity>(),
    //             (query, path) => query.Include(path));
    //     // return included.FirstOrDefault(predicate);
    // }

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await _dataContext.Set<TEntity>().AddAsync(entity, cancellationToken);
        await _dataContext.SaveChangesAsync(cancellationToken);
    }

    public void Update(TEntity entity)
    {
        _dataContext.Set<TEntity>().Update(entity);
        _dataContext.SaveChanges();
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        => _dataContext
            .Set<TEntity>()
            .Remove((await GetAsync(c => c.Id == id, cancellationToken))!);

    public Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = new())
        => _dataContext.Set<TEntity>().AnyAsync(predicate, cancellationToken: cancellationToken);


    public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = new()) =>
        await _dataContext.Set<TEntity>().ToListAsync(cancellationToken: cancellationToken);

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
        await _dataContext.SaveChangesAsync(cancellationToken);

    public async Task AddWithoutSaveAsync(TEntity entity, CancellationToken cancellationToken) =>
        await _dataContext.AddAsync(entity, cancellationToken);
}