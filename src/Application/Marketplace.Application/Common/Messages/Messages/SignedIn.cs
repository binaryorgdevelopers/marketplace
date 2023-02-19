namespace Marketplace.Application.Common.Messages.Messages;

public record SignedIn(Guid Id,string Email, string PhoneNumber, string Role, string FirstName, string LastName);