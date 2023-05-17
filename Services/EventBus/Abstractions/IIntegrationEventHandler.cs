using EventBus.Events;

namespace EventBus.Abstractions;

public interface IIntegrationEventHandler<in TIntegrationsEvent> : IIntegrationEventHandler
    where TIntegrationsEvent : IntegrationEvent
{
    Task Handle(TIntegrationsEvent @event);
}

public interface IIntegrationEventHandler
{
}