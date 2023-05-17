using Davr.Services.Marketplace.Ordering.Domain.Events;
using Ordering.Domain.Events;
using Ordering.Domain.Exceptions;
using Ordering.Domain.SeedWork;

namespace Ordering.Domain.AggregatesModel.OrderAggregate;

public class Order : Entity, IAggregateRoot
{
    private DateTime _orderDate;

    public Address Address { get; private set; }

    public Guid? GetBuyerId => _buyerId;
    private Guid? _buyerId;

    public OrderStatus OrderStatus { get; private set; }
    private Guid _orderStatusId;

    private string _description;

    private readonly List<OrderItem> _orderItems;
    public ICollection<OrderItem> OrderItems => _orderItems;

    private Guid? _paymentMethodId;

    public static Order NewDraft()
    {
        var order = new Order
        {
            _isDraft = true
        };
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

    public Order(Guid userId, string username, Address address, Guid cardTypeId, string cardNumber,
        string cardSecurity, string cardHolderName, DateTime cardExpiration, Guid? buyerId = null,
        Guid? paymentMethodId = null)
    {
        _buyerId = buyerId;
        _paymentMethodId = paymentMethodId;
        _orderStatusId = OrderStatus.Submitted.Id;
        _orderDate = DateTime.UtcNow;
        Address = address;

        // Add the OrderStarterDomainEvent to the domain events collection 
        // to be raised/dispatched when comitting changes into the Database [ After DbContext.SaveChanges() ]
        AddOrderStartedDomainEvent(userId, username, cardTypeId, cardNumber,
            cardSecurity, cardHolderName, cardExpiration);
    }

    public void AddOrderItem(Guid productId, string productName, decimal unitPrice, decimal discount, string pictureUrl,
        int units = 1)
    {
        var existingOrderForProduct = _orderItems.SingleOrDefault(o => o.ProductId == productId);

        if (existingOrderForProduct != null)
        {
            if (discount > existingOrderForProduct.GetCurrentDiscount())
            {
                existingOrderForProduct.SetNewDiscount(discount);
            }

            existingOrderForProduct.AddUnits(units);
        }
        else
        {
            //add validated new order item
            var orderItem = new OrderItem(productId, productName, unitPrice, discount, pictureUrl);
            _orderItems.Add(orderItem);
        }
    }

    public void SetPaymentId(Guid id) => _paymentMethodId = id;
    public void SetBuyerId(Guid id) => _buyerId = id;

    public void SetAwaitingValidationStatus()
    {
        if (_orderStatusId == OrderStatus.AwaitingValidation.Id)
        {
            AddDomainEvent(new OrderStatusChangedToStockConfirmedDomainEvent(Id));

            _orderStatusId = OrderStatus.StockConfirmed.Id;
            _description = "All the items were confirmed with available stock.";
        }
    }

    public void SetPaidStatus()
    {
        if (_orderStatusId == OrderStatus.StockConfirmed.Id)
        {
            AddDomainEvent(new OrderStatusChangedToPaidDomainEvent(Id, OrderItems));

            _orderStatusId = OrderStatus.Paid.Id;
            _description =
                "The payment was performed at a simulated \"American Bank checking bank account ending on XX35071\"";
        }
    }

    public void SetShippedStatus()
    {
        if (_orderStatusId != OrderStatus.Paid.Id)
        {
            StatusChangeException(OrderStatus.Shipped);
        }

        _orderStatusId = OrderStatus.Shipped.Id;
        _description = "The order was shipped.";
        AddDomainEvent(new OrderShippedDomainEvent(this));
    }

    public void SetCancelledStatus()
    {
        if (_orderStatusId == OrderStatus.Paid.Id ||
            _orderStatusId == OrderStatus.Shipped.Id)
        {
            StatusChangeException(OrderStatus.Cancelled);
        }

        _orderStatusId = OrderStatus.Cancelled.Id;
        _description = $"The order was cancelled.";
        AddDomainEvent(new OrderCancelledDomainEvent(this));
    }

    public void SetCancelledStatusWhenStockIsRejected(IEnumerable<Guid> orderStockRejectedItems)
    {
        if (_orderStatusId == OrderStatus.AwaitingValidation.Id)
        {
            _orderStatusId = OrderStatus.Cancelled.Id;

            var itemsStockRejectedProductNames = OrderItems
                .Where(c => orderStockRejectedItems.Contains(c.ProductId))
                .Select(c => c.GetOrderItemProductName());

            var itemsStockRejectedDescription = string.Join(", ", itemsStockRejectedProductNames);
            _description = $"The product items don't have stock: ({itemsStockRejectedDescription}).";
        }
    }

    public void SetStockConfirmedStatus()
    {
        if (_orderStatusId == OrderStatus.AwaitingValidation.Id)
        {
            AddDomainEvent(new OrderStatusChangedToStockConfirmedDomainEvent(Id));
            _orderStatusId = OrderStatus.StockConfirmed.Id;
            _description = "All the items were confirmed with available stock";
        }
    }


    private void AddOrderStartedDomainEvent(Guid userId, string username, Guid cardTypeId, string cardNumber,
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