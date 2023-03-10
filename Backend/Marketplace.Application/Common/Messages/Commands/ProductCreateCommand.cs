using Marketplace.Application.Abstractions.Messaging;

namespace Marketplace.Application.Common.Messages.Commands;

public record ProductCreateCommand(
    Guid SellerId,
    Guid CategoryId,
    string Title,
    string Description,
    IEnumerable<CharacteristicsCreate> Characteristics,
    IEnumerable<BadgeCreate> Badges,
    IEnumerable<BlobCreate> Photos) : ICommand;