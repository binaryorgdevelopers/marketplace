using Ordering.Domain.Exceptions;
using Ordering.Domain.SeedWork;

namespace Ordering.Domain.AggregatesModel.OrderAggregate;

public class OrderStatus : Enumeration
{
    public static OrderStatus Submitted = new OrderStatus(Guid.NewGuid(), nameof(Submitted).ToLowerInvariant());

    public static OrderStatus AwaitingValidation =
        new OrderStatus(Guid.NewGuid(), nameof(AwaitingValidation).ToLowerInvariant());

    public static OrderStatus StockConfirmed =
        new OrderStatus(Guid.NewGuid(), nameof(StockConfirmed).ToLowerInvariant());

    public static OrderStatus Paid = new OrderStatus(Guid.NewGuid(), nameof(Paid).ToLowerInvariant());
    public static OrderStatus Shipped = new OrderStatus(Guid.NewGuid(), nameof(Shipped).ToLowerInvariant());
    public static OrderStatus Cancelled = new OrderStatus(Guid.NewGuid(), nameof(Cancelled).ToLowerInvariant());


    public OrderStatus(Guid id, string name) : base(id, name)
    {
    }


    public static IEnumerable<OrderStatus> List() => new[]
        { Submitted, AwaitingValidation, StockConfirmed, Paid, Shipped, Cancelled };

    public static OrderStatus FromName(string name)
    {
        var state = List().SingleOrDefault(s => string.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));
        if (state == null)
            throw new OrderingDomainException(
                $"Possible values for OrderStatus : {string.Join(",", List().Select(s => s.Name))}");
        return state;
    }

    public static OrderStatus From(Guid id)
    {
        var state = List().SingleOrDefault(s => s.Id == id);
        if (state == null)
            throw new OrderingDomainException(
                $"Possible values for OrderStatus:{string.Join(",", List().Select(c => c.Name))}");

        return state;
    }
}