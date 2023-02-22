namespace Marketplace.Application.Common.Messages.Messages;

public record BlobRead(Guid Id, DateTime CreatedAt, DateTime UpdatedAt, string FileName, string Extras);