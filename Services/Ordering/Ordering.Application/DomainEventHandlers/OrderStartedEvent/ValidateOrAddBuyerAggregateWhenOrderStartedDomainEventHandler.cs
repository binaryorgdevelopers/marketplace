﻿using Davr.Services.Marketplace.Ordering.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.IntegrationEvents;
using Ordering.Application.IntegrationEvents.Events;
using Ordering.Domain.AggregatesModel.BuyerAggregate;

namespace Ordering.Application.DomainEventHandlers.OrderStartedEvent;

public class
    ValidateOrAddBuyerAggregateWhenOrderStartedDomainEventHandler : INotificationHandler<OrderStartedDomainEvent>
{
    private readonly ILoggerFactory _logger;
    private readonly IBuyerRepository _buyerRepository;
    private readonly IOrderingIntegrationEventService _orderingIntegrationEventService;

    public ValidateOrAddBuyerAggregateWhenOrderStartedDomainEventHandler(ILoggerFactory logger,
        IBuyerRepository buyerRepository, IOrderingIntegrationEventService orderingIntegrationEventService)
    {
        _logger = logger;
        _buyerRepository = buyerRepository;
        _orderingIntegrationEventService = orderingIntegrationEventService;
    }

    public async Task Handle(OrderStartedDomainEvent notification, CancellationToken cancellationToken)
    {
        var cardTypeId = (notification.CardTypeId != null) ? notification.CardTypeId : Guid.NewGuid();
        var buyer = await _buyerRepository.FindAsync(notification.UserId);

        bool buyerOriginallyExisted = buyer != null;

        if (!buyerOriginallyExisted) buyer = new Buyer(notification.UserId, notification.UserName);
        buyer.VerifyOrAddPaymentMethod(cardTypeId,
            $"Payment Method on {DateTime.UtcNow}",
            notification.CardNumber,
            notification.CardSecurityNumber,
            notification.CardHolderName,
            notification.CardExpiration,
            notification.Order.Id);

        var buyerUpdated = buyerOriginallyExisted ? _buyerRepository.Update(buyer) : _buyerRepository.Add(buyer);
        await _buyerRepository.UnitOfWork
            .SaveEntitiesAsync(cancellationToken);

        var orderStatusChangedToSubmittedIntegrationEvent =
            new OrderStatusChangedToSubmittedIntegrationEvent(notification.Order.Id,
                notification.Order.OrderStatus.Name, buyer.Name);

        await _orderingIntegrationEventService.AddAndSaveEventAsync(orderStatusChangedToSubmittedIntegrationEvent);


        _logger.CreateLogger<ValidateOrAddBuyerAggregateWhenOrderStartedDomainEventHandler>()
            .LogTrace("Buyer {BuyerId} and related payment method were validated or updated for orderId: {OrderId}",
                buyerUpdated.Id,
                notification.Order.Id);
        throw new NotImplementedException();
    }
}