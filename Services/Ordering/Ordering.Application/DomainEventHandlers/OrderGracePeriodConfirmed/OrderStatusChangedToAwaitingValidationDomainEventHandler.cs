using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.IntegrationEvents;
using Ordering.Application.IntegrationEvents.Events;
using Ordering.Domain.AggregatesModel.BuyerAggregate;
using Ordering.Domain.AggregatesModel.OrderAggregate;
using Ordering.Domain.Events;

namespace Ordering.Application.DomainEventHandlers.OrderGracePeriodConfirmed;

public class OrderStatusChangedToAwaitingValidationDomainEventHandler :
    INotificationHandler<OrderStatusChangedToAwaitingValidationDomainEvent>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ILoggerFactory _logger;
    private readonly IBuyerRepository _buyerRepository;
    private readonly IOrderingIntegrationEventService _orderingIntegrationEventService;

    public OrderStatusChangedToAwaitingValidationDomainEventHandler(IOrderRepository orderRepository,
        ILoggerFactory logger, IBuyerRepository buyerRepository,
        IOrderingIntegrationEventService orderingIntegrationEventService)
    {
        _orderRepository = orderRepository;
        _logger = logger;
        _buyerRepository = buyerRepository;
        _orderingIntegrationEventService = orderingIntegrationEventService;
    }

    public async Task Handle(OrderStatusChangedToAwaitingValidationDomainEvent notification,
        CancellationToken cancellationToken)
    {
        _logger.CreateLogger<OrderStatusChangedToAwaitingValidationDomainEvent>()
            .LogTrace("Order with Id: {OrderId} has been successfully updated to status {Status} ({Id})",
                notification.OrderId, nameof(OrderStatus.AwaitingValidation), OrderStatus.AwaitingValidation.Id);

        var order = await _orderRepository.GetAsync(notification.OrderId);

        var buyer = await _buyerRepository.FindByIdAsync(order.GetBuyerId.Value);

        var orderStockList = notification.OrderItems
            .Select(item => new OrderStockItem(item.GetUnits(), item.ProductId));

        var orderStatusChangedToAwaitingValidationIntegrationEvent =
            new OrderStatusChangedToAwaitingValidationIntegrationEvent(order.Id, order.OrderStatus.Name, buyer.Name,
                orderStockList);
        await _orderingIntegrationEventService.AddAndSaveEventAsync(
            orderStatusChangedToAwaitingValidationIntegrationEvent);
    }
}