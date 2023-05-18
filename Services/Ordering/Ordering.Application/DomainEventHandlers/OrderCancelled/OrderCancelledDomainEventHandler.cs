using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.IntegrationEvents;
using Ordering.Application.IntegrationEvents.Events;
using Ordering.Domain.AggregatesModel.BuyerAggregate;
using Ordering.Domain.AggregatesModel.OrderAggregate;
using Ordering.Domain.Events;

namespace Ordering.Application.DomainEventHandlers.OrderCancelled;

public class OrderCancelledDomainEventHandler : INotificationHandler<OrderCancelledDomainEvent>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IBuyerRepository _buyerRepository;
    private readonly ILoggerFactory _logger;
    private readonly IOrderingIntegrationEventService _orderingIntegrationEventService;

    public OrderCancelledDomainEventHandler(IOrderRepository orderRepository, IBuyerRepository buyerRepository,
        ILoggerFactory logger, IOrderingIntegrationEventService orderingIntegrationEventService)
    {
        _orderRepository = orderRepository;
        _buyerRepository = buyerRepository;
        _logger = logger;
        _orderingIntegrationEventService = orderingIntegrationEventService;
    }

    public async Task Handle(OrderCancelledDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.CreateLogger<OrderCancelledDomainEvent>()
            .LogTrace("Order with Id: {OrderId} had been successfully updated to status {Status} ({Id}) ",
                notification.Order.Id, nameof(OrderStatus.Cancelled), OrderStatus.Cancelled.Id);

        var order = await _orderRepository.GetAsync(notification.Order.Id);
        var buyer = await _buyerRepository.FindByIdAsync(order.GetBuyerId.Value);

        var orderStatusChangedToCancelledIntegrationEvent =
            new OrderStatusChangeToCancelledIntegrationEvent(order.Id, order.OrderStatus.Name, buyer.Name);
        await _orderingIntegrationEventService.AddAndSaveEventAsync(orderStatusChangedToCancelledIntegrationEvent);
    }
}