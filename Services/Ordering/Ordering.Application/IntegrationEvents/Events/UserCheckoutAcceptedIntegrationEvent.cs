﻿using EventBus.Events;
using Ordering.Application.Models;

namespace Ordering.Application.IntegrationEvents.Events;

public record UserCheckoutAcceptedIntegrationEvent : IntegrationEvent
{
    public Guid UserId { get; }
    public string UserName { get; }
    public string City { get; set; }
    public string Street { get; set; }
    public string State { get; set; }
    public string Country { get; set; }
    public string ZipCode { get; set; }
    public string CardNumber { get; set; }
    public string CardHolderName { get; set; }
    public DateTime CardExpiration { get; set; }
    public string CardSecurityNumber { get; set; }
    public Guid CardTypeId { get; set; }
    public string Buyer { get; set; }
    public Guid RequestId { get; set; }
    public CustomerBasket Basket { get; }

    public UserCheckoutAcceptedIntegrationEvent(Guid userId, string userName, string city, string street, string state,
        string country, string zipCode, string cardNumber, string cardHolderName, DateTime cardExpiration,
        string cardSecurityNumber, Guid cardTypeId, string buyer, Guid requestId, CustomerBasket basket)
    {
        UserId = userId;
        UserName = userName;
        City = city;
        Street = street;
        State = state;
        Country = country;
        ZipCode = zipCode;
        CardNumber = cardNumber;
        CardHolderName = cardHolderName;
        CardExpiration = cardExpiration;
        CardSecurityNumber = cardSecurityNumber;
        CardTypeId = cardTypeId;
        Buyer = buyer;
        RequestId = requestId;
        Basket = basket;
    }
}