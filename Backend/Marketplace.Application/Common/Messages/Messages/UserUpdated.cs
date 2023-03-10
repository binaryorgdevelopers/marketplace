namespace Marketplace.Application.Common.Messages.Messages;

public record UserUpdated(Guid Id, DateTime CreatedAt, DateTime UpdatedAt, string Role, string FirstName,
    string PhoneNumber, string Email);