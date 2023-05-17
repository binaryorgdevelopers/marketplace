using Microsoft.EntityFrameworkCore;
using Ordering.Domain.AggregatesModel.BuyerAggregate;
using Ordering.Domain.SeedWork;

namespace Ordering.Infrastructure.Repositories;

public class BuyerRepository : IBuyerRepository
{
    private readonly OrderingContext _context;

    public IUnitOfWork UnitOfWork => _context;

    public BuyerRepository(OrderingContext context)
    {
        _context = context;
    }

    public Buyer Add(Buyer buyer) => buyer.IsTransient()
        ? _context
            .Buyers
            .Add(buyer)
            .Entity
        : buyer;


    public Buyer Update(Buyer buyer) => _context
        .Buyers
        .Update(buyer)
        .Entity;

    public async Task<Buyer> FindAsync(Guid identity)
    {
        var buyer = await _context.Buyers
            .Include(b => b.PaymentMethods)
            .Where(b => b.IdentityGuid == identity)
            .SingleOrDefaultAsync();
        return buyer;
    }

    public async Task<Buyer> FindByIdAsync(Guid id)
    {
        var buyer = await _context.Buyers
            .Include(b => b.PaymentMethods)
            .Where(b => b.Id == id)
            .SingleOrDefaultAsync();
        return buyer;
    }
}