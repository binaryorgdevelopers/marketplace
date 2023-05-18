using Inventory.Domain.Entities;

namespace Marketplace.Application.Common.Messages.Messages;

public record CardDetailsDto(
    string CardNumber,
    string ExpiryMonth,
    string ExpiryYear,
    string Cvv,
    string CardHolderName
)
{
    public static CardDetailsDto FromCardDetails(CardDetail cardDetail) =>
        new(
            cardDetail.Cn,
            cardDetail.Em,
            cardDetail.Ey,
            cardDetail.Cv,
            cardDetail.Chn);
}