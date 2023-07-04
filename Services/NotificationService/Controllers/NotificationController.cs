using Microsoft.AspNetCore.Mvc;
using NotificationService.Hubs;

namespace NotificationService.Controllers;

[ApiController]
[Route("notification")]
public class NotificationController : ControllerBase
{
    private readonly IHubWrapper _hubWrapper;

    public NotificationController(IHubWrapper hubWrapper)
    {
        _hubWrapper = hubWrapper;
    }

    [HttpPost("push")]
    public async Task<ActionResult> PushNotification(NotificationPayload payload)
    {
        await _hubWrapper.PublishToAllAsync(payload.Message, payload.Data);
        return Ok();
    }
}

public record NotificationPayload(string Message, object Data);