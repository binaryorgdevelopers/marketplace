namespace Marketplace.Application.Common.Messages.Commands;

public record CategoryCreate(
    Guid Id,
    string Title
);