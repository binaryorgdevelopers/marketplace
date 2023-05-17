namespace Marketplace.Application.Common.Messages.Commands;

public record BlobCreate(
    string file,
    string? Extras
);