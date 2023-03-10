using System.Linq.Expressions;
using Marketplace.Domain.Abstractions;
using Marketplace.Domain.Abstractions.Repositories;
using Marketplace.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Infrastructure.Repositories;

/// <summary>
/// Provides methods for generic entity repository base to manipulate data
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
            .FirstOrDefaultAsync(predicate);


    public TEntity? Get(Expression<Func<TEntity, bool>> predicate)
        => _dataContext.Set<TEntity>().FirstOrDefault(predicate);

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

    public IEnumerable<TSelect> GetWithInclude<TProperty, TSelect>(
        Expression<Func<TEntity, TProperty>> include,
        Expression<Func<TEntity, TSelect>> select)
        => _dataContext
            .Set<TEntity>()
            .Include(include)
            .AsSplitQuery()
            .Select(select);

    public TSelect? GetSingleWithInclude<TProperty, TSelect>(
        Expression<Func<TEntity, TProperty>> include,
        Expression<Func<TEntity, TSelect>> select,
        Expression<Func<TSelect?, bool>> predicate) =>
        _dataContext.Set<TEntity>()
            .Include(include)
            .Select(select)
            .AsEnumerable()
            .FirstOrDefault(predicate.Compile());


    public IQueryable<TEntity> IncludeMultiple<TProperty>(
        params Expression<Func<TEntity, TProperty>>[] includes)
        where TProperty : class
    {
        var queryable = _dataContext.Set<TEntity>().AsQueryable();
        var result = includes
            .Aggregate(queryable, (current, func) =>
                current.Include(func));
        return result;
    }
}