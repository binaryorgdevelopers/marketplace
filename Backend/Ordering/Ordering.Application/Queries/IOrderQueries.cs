
namespace Ordering.Application.Queries;

public interface IOrderQueries
{
    Task<Order> GetOrderAsync(Guid id);
    Task<IEnumerable<OrderSummary>> GetOrderFromUserAsync(Guid? userId);
    Task<IEnumerable<CardType>> GetCardTypeAsync();
}