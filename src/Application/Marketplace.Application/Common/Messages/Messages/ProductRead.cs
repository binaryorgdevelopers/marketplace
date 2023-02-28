namespace Marketplace.Application.Common.Messages.Messages;

public record ProductRead(
    Guid Id,
    string[]? Attributes,
    IEnumerable<BadgeRead> Badges,
    string? Synonyms,
    string Title,
    string Description,
    CategoryRead Category,
    SellerRead Seller,
    IEnumerable<BlobRead> Photos,
    IEnumerable<CharacteristicsRead> Characteristics);