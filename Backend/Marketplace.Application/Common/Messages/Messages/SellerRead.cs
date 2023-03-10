namespace Marketplace.Application.Common.Messages.Messages;

public record SellerRead(
    Guid Id,
    string Title,
    string Description,
    string Info,
    string Username,
    string FirstName,
    string LastName,
    string Banner,
    string Avatar,
    string Link
);