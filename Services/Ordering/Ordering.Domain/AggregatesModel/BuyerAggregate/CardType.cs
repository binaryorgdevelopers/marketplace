using Ordering.Domain.SeedWork;

namespace Ordering.Domain.AggregatesModel.BuyerAggregate;

/// <remarks> 
/// Card type class should be marked as abstract with protected constructor to encapsulate known enum types
/// this is currently not possible as OrderingContextSeed uses this constructor to load cardTypes from csv file
/// </remarks>
public class CardType
    : Enumeration
{
    public static CardType Amex = new(Guid.NewGuid(), nameof(Amex));
    public static CardType Visa = new(Guid.NewGuid(), nameof(Visa));
    public static CardType MasterCard = new(Guid.NewGuid(), nameof(MasterCard));

    public CardType(Guid id, string name)
        : base(id, name)
    {
    }
}