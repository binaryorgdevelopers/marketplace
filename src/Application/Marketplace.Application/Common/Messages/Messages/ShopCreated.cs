namespace Marketplace.Application.Common.Messages.Messages;

public record ShopCreated(
    Guid UserId,
    string Name,
    int Number,
    string Extras 
);