using NotificationService.Persistence.Entities;
using Shared.Abstraction.MediatR;

namespace NotificationService.Dtos;

public record NotificationRequest(
    Guid? UserId,
    string Title,
    string Message,
    string MessageContent
) : IRequest<Notification>;