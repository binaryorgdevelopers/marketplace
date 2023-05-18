using Shared.Abstraction;

namespace NotificationService.Persistence.Entities;

public class Notification : IIdentifiable
{
    public Guid Id { get; set; }
    public string? User { get; }
    public string Title { get; set; }
    public string Key { get; set; } = string.Empty;
    public string Message { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? SentAt { get; set; }
    public string MessageContent { get; set; }

    private Notification(string? user, string title, string message, string messageContent, DateTime? sentAt)
    {
        User = user;
        Title = title;
        Message = message;
        MessageContent = messageContent;
        SentAt = sentAt ?? DateTime.Now;
    }

    public Notification()
    {
    }

    public static Notification Create(string? user, string title, string message, string messageContent,
        DateTime? sentAt = null) =>
        new(user, title, message, messageContent, sentAt);
}