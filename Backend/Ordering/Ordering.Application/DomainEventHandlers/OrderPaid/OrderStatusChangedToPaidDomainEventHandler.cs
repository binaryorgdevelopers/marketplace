using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.IntegrationEvents;
using Ordering.Application.IntegrationEvents.Events;
using Ordering.Domain.AggregatesModel.BuyerAggregate;
using Ordering.Domain.AggregatesModel.OrderAggregate;
using Ordering.Domain.Events;

namespace Ordering.Application.DomainEventHandlers.OrderPaid;

public class OrderStatusChangedToPaidDomainEventHandler : INotificationHandler<OrderStatusChangedToPaidDomainEvent>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ILoggerFactory _logger;
    private readonly IBuyerRepository _buyerRepository;
    private readonly IOrderingIntegrationEventService _orderingIntegrationEventService;

    public OrderStatusChangedToPaidDomainEventHandler(IOrderRepository orderRepository, ILoggerFactory logger,
        IBuyerRepository buyerRepository, IOrderingIntegrationEventService orderingIntegrationEventService)
    {
        _orderRepository = orderRepository;
        _logger = logger;
        _buyerRepository = buyerRepository;
        _orderingIntegrationEventService = orderingIntegrationEventService;
    }

    public async Task Handle(OrderStatusChangedToPaidDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.CreateLogger<OrderStatusChangedToPaidDomainEventHandler>()
            .LogTrace("Order with Id: {OrderId} has been successfully updated to status {Status} ({Id})",
                notification.OrderId, nameof(OrderStatus.Paid), OrderStatus.Paid.Id);
        var order = await _orderRepository.GetAsync(notification.OrderId);
        var buyer = await _buyerRepository.FindByIdAsync(order.GetBuyerId.Value);

        var orderStockList = notification.OrderItems
            .Select(orderItem => new OrderStockItem(orderItem.GetUnits(), orderItem.ProductId));

        var orderStatusChangedToPaidIntegrationEvent = new OrderStatusChangedToPaidIntegrationEvent(
            notification.OrderId,
            order.OrderStatus.Name,
            buyer.Name,
            orderStockList);

        await _orderingIntegrationEventService.AddAndSaveEventAsync(orderStatusChangedToPaidIntegrationEvent);
    }
}