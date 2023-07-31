using Inventory.Domain.Abstractions.Repositories;
using Inventory.Domain.Entities;
using Marketplace.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly DataContext _context;

    public ProductRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<Product?> GetById(Guid id)
    {
        return await _context.Set<Product>()
            .Where(p => p.Id == id)
            .AsSplitQuery()
            .Include(c => c.Characteristics)
            .Include(c => c.Badges)
            .Include(c => c.Photos)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
}