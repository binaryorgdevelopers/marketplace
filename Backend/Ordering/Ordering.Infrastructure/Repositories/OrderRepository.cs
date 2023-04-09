using Microsoft.EntityFrameworkCore;
using Ordering.Domain.AggregatesModel.OrderAggregate;
using Ordering.Domain.SeedWork;

namespace Ordering.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly OrderingContext _context;
    public IUnitOfWork UnitOfWork => _context;

    public OrderRepository(OrderingContext context)
    {
        _context = context;
    }

    public Order Add(Order order) =>
        _context.Orders.Add(order).Entity;

    public void Update(Order order)
    {
        _context.Entry(order).State = EntityState.Modified;
    }

    public async Task<Order> GetAsync(int orderId)
    {
        var order = await _context
            .Orders
            .Include(x => x.Address)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order! == null!)
        {
            order = _context
                .Orders
                .Local
                .FirstOrDefault(o => o.Id == orderId);
        }

        if (order! != null!)
        {
            await _context.Entry(order)
                .Collection(i => i.OrderItems).LoadAsync();
            await _context.Entry(order)
                .Reference(i => i.OrderStatus).LoadAsync();
        }

        return order!;
    }
}