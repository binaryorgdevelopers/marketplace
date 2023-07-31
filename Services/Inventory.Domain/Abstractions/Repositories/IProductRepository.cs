using Inventory.Domain.Entities;

namespace Inventory.Domain.Abstractions.Repositories;


public interface IProductRepository
{
    Task<Product?> GetById(Guid id);
}