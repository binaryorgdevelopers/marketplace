﻿using EventBus.Events;

namespace Ordering.Application.IntegrationEvents.Events;

public record OrderStartedIntegrationEvent : IntegrationEvent
{
    public string UserId { get; init; }

    public OrderStartedIntegrationEvent(string userId) => UserId = userId;
}