using Marketplace.Application.Abstractions.Messaging;
using Marketplace.Application.Common.Messages.Messages;

namespace Marketplace.Application.Common.Messages.Commands;

public record ProductCreateCommand(
    Guid SellerId,
    Guid CategoryId,
    string Title,
    decimal Price,
    int Count,
    string Description,
    IEnumerable<CharacteristicsCreate> Characteristics,
    IEnumerable<BadgeCreate> Badges,
    IEnumerable<BlobCreate> Photos) : ICommand<ProductDto>;