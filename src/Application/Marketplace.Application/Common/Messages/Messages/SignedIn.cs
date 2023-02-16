namespace Marketplace.Application.Common.Messages.Messages;

public record SignedIn(string Email, string PhoneNumber, string Role, string FirstName, string LastName);