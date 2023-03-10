using Microsoft.AspNetCore.Http;

namespace Marketplace.Application.Common.Messages.Commands;

public record BlobCreate(
    IFormFile file,
    string? Extras
);