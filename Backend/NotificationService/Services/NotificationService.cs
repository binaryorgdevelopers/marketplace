using NotificationService.DAL.Entities;
using NotificationService.Dtos;
using Shared.Abstraction.MediatR;

namespace NotificationService.Services;

public class NotificationService : INotificationService
{
    private readonly IRequestHandler<NotificationRequest, Notification> _addNotificationRequestHandler;
    private readonly ILogger<NotificationService> _logger;

    public NotificationService(
        IRequestHandler<NotificationRequest, Notification> addNotificationRequestHandler,
        ILogger<NotificationService> logger)
    {
        _addNotificationRequestHandler = addNotificationRequestHandler;
        _logger = logger;
    }

    public Task SendNotificationAsync(string textMessage)
    {
        _logger.LogInformation(textMessage);
        return Task.CompletedTask;
    }
}