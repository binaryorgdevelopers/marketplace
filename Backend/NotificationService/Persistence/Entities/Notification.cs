using Shared.Abstraction;

namespace NotificationService.Persistence.Entities;

public class Notification : IIdentifiable
{
    public Guid Id { get; set; }
    public Guid? UserId { get; }
    public string Title { get; set; }
    public string Key { get; set; }
    public string Message { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? SentAt { get; set; }
    public string MessageContent { get; set; }

    private Notification(Guid? userId, string title, string message, string messageContent,DateTime sentAt)
    {
        UserId = userId;
        Title = title;
        Message = message;
        MessageContent = messageContent;
        SentAt = sentAt;
    }

    public Notification()
    {
        
    }

    public static Notification Create(Guid? userId, string title, string message, string messageContent,DateTime sentAt) =>
        new(userId, title, message, messageContent,sentAt);
}