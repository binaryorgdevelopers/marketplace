using EventBus.Abstractions;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Hubs;
using NotificationService.IntegrationEvents.Events;
using NotificationService.Persistence;
using NotificationService.Persistence.Entities;

namespace NotificationService.IntegrationEvents.EventHandling;

internal class
    OrderStatusChangedToAwaitingValidationEventHandler : IIntegrationEventHandler<
        OrderStatusChangedToAwaitingValidationIntegrationEvent>
{
    private readonly NotificationsHub _hubContext;
    private readonly ILogger<OrderStatusChangedToAwaitingValidationEventHandler> _logger;
    private readonly NotificationContext _context;

    public OrderStatusChangedToAwaitingValidationEventHandler(NotificationsHub hubContext,
        ILogger<OrderStatusChangedToAwaitingValidationEventHandler> logger, NotificationContext context)
    {
        _hubContext = hubContext;
        _logger = logger;
        _context = context;
    }

    public async Task Handle(OrderStatusChangedToAwaitingValidationIntegrationEvent @event)
    {
        _logger.LogInformation(
            "----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})",
            @event.Id, Program.AppName,
            @event);

        await _hubContext
            .Clients
            .Group(@event.BuyerName)
            .SendAsync("UpdatedOrderState", new { @event.OrderId, Status = @event.OrderStatus });

        await _context.Notifications.AddAsync(Notification.Create(@event.BuyerName, @event.BuyerName,
            "UpdatedOrderState", $"Your order with Id:'{@event.OrderId} is awaiting validation'"));
    }
}