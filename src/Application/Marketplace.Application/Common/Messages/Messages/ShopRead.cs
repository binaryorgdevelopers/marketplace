namespace Marketplace.Application.Common.Messages.Messages;

public record ShopRead(
    Guid Id,
    string Name,
    int Number,
    string Extras
);