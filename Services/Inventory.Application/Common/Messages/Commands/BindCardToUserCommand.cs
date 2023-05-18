using Inventory.Domain.Entities;
using Marketplace.Application.Abstractions.Messaging;

namespace Marketplace.Application.Common.Messages.Commands;

public record BindCardToUserCommand(Guid UserId, IList<CardDetailCreateCommand> Cards) : ICommand
{
    public IEnumerable<CardDetail> ToCardDetails() =>
        Cards.Select(c =>
            CardDetail.Create(c.CardNumber, c.ExpirationMonth, c.ExpirationYear, c.Cvv, c.CardHolderName, UserId));
};

public record CardDetailCreateCommand(string CardNumber, string ExpirationMonth, string ExpirationYear,
    string Cvv, string CardHolderName);