using EventBus.Abstractions;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Hubs;
using NotificationService.IntegrationEvents.Events;
using NotificationService.Persistence;
using NotificationService.Persistence.Entities;

namespace NotificationService.IntegrationEvents.EventHandling;

public class
    OrderStatusChangedToCancelledEventHandler : IIntegrationEventHandler<OrderStatusChangedToCancelledIntegrationEvent>
{
    private readonly NotificationsHub _hubContext;
    private readonly ILogger<OrderStatusChangedToCancelledEventHandler> _logger;
    private readonly NotificationContext _context;

    public OrderStatusChangedToCancelledEventHandler(NotificationsHub hubContext,
        ILogger<OrderStatusChangedToCancelledEventHandler> logger, NotificationContext context)
    {
        _hubContext = hubContext;
        _logger = logger;
        _context = context;
    }

    public async Task Handle(OrderStatusChangedToCancelledIntegrationEvent @event)
    {
        _logger.LogInformation(
            "----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})",
            @event.Id, Program.AppName,
            @event);
        await _hubContext
            .Clients
            .Group(@event.BuyerName)
            .SendAsync("UpdatedOrderState", new { @event.OrderId, Status = @event.OrderId });

        await _context.Notifications.AddAsync(Notification.Create(@event.BuyerName, @event.BuyerName,
            "UpdatedOrderState", $"Your order with Id:'{@event.OrderId} is cancelled'"));
    }
}