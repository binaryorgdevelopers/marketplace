using MediatR;
using Ordering.Domain.AggregatesModel.OrderAggregate;

// ReSharper disable All

namespace Davr.Services.Marketplace.Ordering.Domain.Events;

/// <summary>
/// Event used when order is created
/// </summary>
public class OrderStartedDomainEvent : INotification
{
    public Guid UserId { get; }
    public string UserName { get; }
    public Guid CardTypeId { get; }
    public string CardNumber { get; }
    public string CardSecurityNumber { get; }
    public string CardHolderName { get; }
    public DateTime CardExpiration { get; }
    public Order Order { get; }


    public OrderStartedDomainEvent(Order order, Guid userId, string userName,
        Guid cardTypeId, string cardNumber,
        string cardSecurityNumber, string cardHolderName,
        DateTime cardExpiration)
    {
        Order = order;
        UserId = userId;
        UserName = userName;
        CardTypeId = cardTypeId;
        CardNumber = cardNumber;
        CardSecurityNumber = cardSecurityNumber;
        CardHolderName = cardHolderName;
        CardExpiration = cardExpiration;
    }
}