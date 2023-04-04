using Marketplace.Application.Common.Builder.Extensions;
using Marketplace.Application.Common.Builder.Models;
using Inventory.Domain.Abstractions;
using Inventory.Domain.Abstractions.Repositories;

namespace Marketplace.Infrastructure.Services;

public class SearchService<TEntity> where TEntity : class, IIdentifiable
{
    private readonly IGenericRepository<TEntity> _genericRepository;

    public SearchService(IGenericRepository<TEntity> genericRepository)
    {
        _genericRepository = genericRepository;
    }

    public async Task<IEnumerable<TEntity>> GetByFilterAsync(IEntityQueryOptions<TEntity> queryOptions)
    {
        ArgumentNullException.ThrowIfNull(queryOptions);
        return await Task.Run(() => { return _genericRepository.Get(x => true).ApplyQuery(queryOptions).ToList(); });
    }
}