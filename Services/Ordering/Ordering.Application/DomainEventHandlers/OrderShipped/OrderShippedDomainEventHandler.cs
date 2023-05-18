using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.IntegrationEvents;
using Ordering.Application.IntegrationEvents.Events;
using Ordering.Domain.AggregatesModel.BuyerAggregate;
using Ordering.Domain.AggregatesModel.OrderAggregate;
using Ordering.Domain.Events;

namespace Ordering.Application.DomainEventHandlers.OrderShipped;

public class OrderShippedDomainEventHandler : INotificationHandler<OrderShippedDomainEvent>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IBuyerRepository _buyerRepository;
    private readonly IOrderingIntegrationEventService _orderingIntegrationEventService;
    private readonly ILoggerFactory _logger;

    public OrderShippedDomainEventHandler(IOrderRepository orderRepository, IBuyerRepository buyerRepository,
        IOrderingIntegrationEventService orderingIntegrationEventService, ILoggerFactory logger)
    {
        _orderRepository = orderRepository;
        _buyerRepository = buyerRepository;
        _orderingIntegrationEventService = orderingIntegrationEventService;
        _logger = logger;
    }

    public async Task Handle(OrderShippedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.CreateLogger<OrderShippedDomainEvent>()
            .LogTrace("Order with Id: {OrderId} has been successfully updated to status {Status} ({Id})",
                notification.Order.Id, nameof(OrderStatus.Shipped), OrderStatus.Shipped.Id);

        var order = await _orderRepository.GetAsync(notification.Order.Id);
        var buyer = await _buyerRepository.FindByIdAsync(order.GetBuyerId.Value);

        var orderStatusChangedToShippedIntegrationEvent = new OrderStatusChangedToShippedIntegrationEvent(
            order.Id, order.OrderStatus.Name, buyer.Name);
        await _orderingIntegrationEventService.AddAndSaveEventAsync(orderStatusChangedToShippedIntegrationEvent);
    }
}