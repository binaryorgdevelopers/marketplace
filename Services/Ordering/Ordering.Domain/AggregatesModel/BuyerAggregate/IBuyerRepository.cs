using Ordering.Domain.SeedWork;

namespace Ordering.Domain.AggregatesModel.BuyerAggregate;

public interface IBuyerRepository : IRepository<Buyer>
{
    Buyer Add(Buyer buyer);
    Buyer Update(Buyer buyer);
    Task<Buyer> FindAsync(Guid identity);
    Task<Buyer> FindByIdAsync(Guid id);
}