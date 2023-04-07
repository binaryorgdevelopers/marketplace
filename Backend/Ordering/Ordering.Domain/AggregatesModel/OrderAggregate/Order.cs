using Ordering.Domain.Events;
using Ordering.Domain.Exceptions;
using Ordering.Domain.SeedWork;

namespace Ordering.Domain.AggregatesModel.OrderAggregate;

public class Order : Entity, IAggregateRoot
{
    private DateTime _orderDate;

    public Address Address { get; private set; }

    public int? GetBuyerId => _buyerId;
    private int? _buyerId;

    private OrderStatus OrderStatus { get; set; }
    private int _orderStatusId;

    private string _description;

    private readonly List<OrderItem> _orderItems;
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

    private int? _paymentMethodId;

    public static Order NewDraft()
    {
        var order = new Order();
        order._isDraft = true;
        return order;
    }

    /// <summary>
    /// Checks order whether drafted or not
    /// </summary>
    private bool _isDraft;

    protected Order()
    {
        _orderItems = new List<OrderItem>();
        _isDraft = false;
    }

    public Order(string userId, string username, Address address, int cardTypeId, string cardNumber,
        string cardSecurity, string cardHolderName, DateTime cardExpiration, int? buyerId = null,
        int? paymentMethodId = null)
    {
        _buyerId = buyerId;
        _paymentMethodId = paymentMethodId;
        _orderStatusId = OrderStatus.Submitted.Id;
        _orderDate = DateTime.UtcNow;
        Address = address;
    }


    private void AddStartedDomainEvent(string userId, string username, int cardTypeId, string cardNumber,
        string cardSecurityNumber, string cardHolderName, DateTime cardExpiration)

    {
        var orderStartedDomainEvent = new OrderStartedDomainEvent(this, userId, username, cardTypeId, cardNumber,
            cardSecurityNumber, cardHolderName, cardExpiration);

        this.AddDomainEvent(orderStartedDomainEvent);
    }

    private void StatusChangeException(OrderStatus orderStatusToChange)
    {
        throw new OrderingDomainException(
            $"Is not possible to change the order status from {OrderStatus.Name} to {orderStatusToChange.Name}.");
    }

    public decimal GetTotal() => _orderItems.Sum(o => o.GetUnits() * o.GetUnitPrice());
}