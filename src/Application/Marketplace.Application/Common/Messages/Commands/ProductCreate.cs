using Microsoft.AspNetCore.Http;

namespace Marketplace.Application.Common.Messages.Commands;

public record ProductCreate(
    Guid SellerId,
    Guid CategoryId,
    string Title,
    string Description,
    IEnumerable<CharacteristicsCreate> Characteristics,
    IEnumerable<BadgeCreate> Badges,
    IEnumerable<BlobCreate> Photos);

public record BlobCreate(
    IFormFile file,
    string? Extras
);