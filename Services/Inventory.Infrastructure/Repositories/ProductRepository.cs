using Inventory.Domain.Entities;

namespace Marketplace.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
}

public interface IProductRepository
{
    Task<Product> GetById(Guid id);
    Task<Product> Update(Guid id);
    Task<Product> AddAsync(Product product);
}