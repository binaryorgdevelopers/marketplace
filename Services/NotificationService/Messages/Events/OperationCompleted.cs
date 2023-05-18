using Newtonsoft.Json;
using Shared.Messages;

namespace NotificationService.Messages.Events;

[MessageNamespace("operations")]
public class OperationCompleted : IEvent
{
    public Guid Id { get; }
    public Guid UserId { get; }
    public string Name { get; }
    public string Resource { get; }

    [JsonConstructor]
    public OperationCompleted(Guid id,
        Guid userId, string name, string resource)
    {
        Id = id;
        UserId = userId;
        Name = name;
        Resource = resource;
    }
}