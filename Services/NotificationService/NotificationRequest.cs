using MediatR;
using NotificationService.Persistence.Entities;

namespace NotificationService;

public record NotificationRequest(
    Guid? UserId,
    string Title,
    string Message,
    string MessageContent
) : IRequest<Notification>;