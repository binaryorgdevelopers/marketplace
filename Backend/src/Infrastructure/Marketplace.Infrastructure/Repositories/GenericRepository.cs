using System.Linq.Expressions;
using Marketplace.Domain.Abstractions;
using Marketplace.Domain.Abstractions.Repositories;
using Marketplace.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Infrastructure.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity>
    where TEntity : class, IIdentifiable, new()
{
    private readonly DataContext _dataContext;

    public GenericRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }


    public async Task<TEntity?> GetAsync(Guid id)
        => await GetAsync(e => e.Id == id);

    public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate)
        => await _dataContext
            .Set<TEntity>()
            .FirstOrDefaultAsync(predicate);

    public async Task<IQueryable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
    {
        await Task.CompletedTask;
        return _dataContext.Set<TEntity>()
            .Where(predicate)
            .AsQueryable();
    }

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
            .Remove((await GetAsync(id))!);

    public Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
        => _dataContext.Set<TEntity>().AnyAsync(predicate);

    public IEnumerable<TEntity> GetAll<TProperty>(Expression<Func<TEntity, TProperty>> predicate)
        => _dataContext
            .Set<TEntity>()
            .AsSplitQuery()
            .Include(predicate);

    public async Task<IEnumerable<TSelect>> GetWithInclude<TProperty, TSelect>(
        Expression<Func<TEntity, TProperty>> include,
        Expression<Func<TEntity, TSelect>> select)
        => await _dataContext
            .Set<TEntity>()
            .Include(include)
            .AsSplitQuery()
            .Select(select).ToListAsync();
}