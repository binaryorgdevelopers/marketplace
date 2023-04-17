
namespace Ordering.Application.Queries;

public interface IOrderQueries
{
    Task<Order> GetOrderAsync(int id);
    Task<IEnumerable<OrderSummary>> GetOrderFromUserAsync(Guid userId);
    Task<IEnumerable<CardType>> GetCardTypeAsync();
}