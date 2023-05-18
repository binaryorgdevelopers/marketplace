using EventBus.Events;

namespace Ordering.Application.IntegrationEvents.Events;

public record TokenRequestEvent(string Token) : IntegrationEvent;