namespace Marketplace.Application.Common.Messages.Commands;

public record ShopCreate(
    Guid UserId,
    string Name,
    int Number,
    string Extras
);