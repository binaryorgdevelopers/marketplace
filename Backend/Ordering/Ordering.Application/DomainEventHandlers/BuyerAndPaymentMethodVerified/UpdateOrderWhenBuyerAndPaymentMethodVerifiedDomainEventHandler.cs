using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Domain.AggregatesModel.OrderAggregate;
using Ordering.Domain.Events;

namespace Ordering.Application.DomainEventHandlers.BuyerAndPaymentMethodVerified;

public class
    UpdateOrderWhenBuyerAndPaymentMethodVerifiedDomainEventHandler : INotificationHandler<
        BuyerAndPaymentMethodVerifiedDomainEvent>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ILoggerFactory _logger;


    public UpdateOrderWhenBuyerAndPaymentMethodVerifiedDomainEventHandler(IOrderRepository orderRepository,
        ILoggerFactory logger)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    // Domain Logic comment
    // When the Buyer and Buyer's payment method have been created or verified that they existed,
    // then we can update the original Order with the BuyerId and PaymentId (foreign keys)

    public async Task Handle(BuyerAndPaymentMethodVerifiedDomainEvent notification, CancellationToken cancellationToken)
    {
        var orderToUpdate = await _orderRepository.GetAsync(notification.OrderId);

        orderToUpdate.SetBuyerId(notification.Buyer.Id);
        orderToUpdate.SetPaymentId(notification.Payment.Id);

        _logger.CreateLogger<UpdateOrderWhenBuyerAndPaymentMethodVerifiedDomainEventHandler>()
            .LogTrace(
                "Order with Id : {OrderId} has been successfully updated with a payment method {PaymentMethod} ({Id})",
                notification.OrderId, nameof(notification.Payment), notification.Payment.Id);
    }
}