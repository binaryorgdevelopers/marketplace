using System.Runtime.Serialization;
using MediatR;
using Ordering.Application.Extensions;
using Ordering.Application.Models;

namespace Ordering.Application.Commands;

[DataContract]
public class CreateOrderCommand : IRequest<bool>
{
    [DataMember] private readonly List<OrderItemDTO> _orderItems;

    [DataMember] public Guid UserId { get; private set; }

    [DataMember] public string UserName { get; private set; }

    [DataMember] public string City { get; private set; }

    [DataMember] public string Street { get; private set; }

    [DataMember] public string State { get; private set; }

    [DataMember] public string Country { get; private set; }

    [DataMember] public string ZipCode { get; private set; }

    [DataMember] public string CardNumber { get; private set; }

    [DataMember] public string CardHolderName { get; private set; }

    [DataMember] public DateTime CardExpiration { get; private set; }

    [DataMember] public string CardSecurityNumber { get; private set; }

    [DataMember] public Guid CardTypeId { get; private set; }

    [DataMember] public IEnumerable<OrderItemDTO> OrderItems => _orderItems;

    public CreateOrderCommand()
    {
        _orderItems = new List<OrderItemDTO>();
    }

    public CreateOrderCommand(List<BasketItem> basketItems, Guid userId, string userName, string city, string street,
        string state, string country, string zipcode,
        string cardNumber, string cardHolderName, DateTime cardExpiration,
        string cardSecurityNumber, Guid cardTypeId) : this()
    {
        _orderItems = basketItems.ToOrderItemsDto().ToList();
        UserId = userId;
        UserName = userName;
        City = city;
        Street = street;
        State = state;
        Country = country;
        ZipCode = zipcode;
        CardNumber = cardNumber;
        CardHolderName = cardHolderName;
        CardExpiration = cardExpiration;
        CardSecurityNumber = cardSecurityNumber;
        CardTypeId = cardTypeId;
    }


    public record OrderItemDTO
    {
        public Guid ProductId { get; init; }

        public string ProductName { get; init; }

        public decimal UnitPrice { get; init; }

        public decimal Discount { get; init; }

        public int Units { get; init; }

        public string PictureUrl { get; init; }
    }
}