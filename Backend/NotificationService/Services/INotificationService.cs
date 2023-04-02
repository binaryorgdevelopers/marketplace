namespace NotificationService.Services;

public interface INotificationService
{
    public Task SendNotificationAsync(string textMessage);
}