namespace Marketplace.Application.Common.Messages.Messages;

public record SignedUp(Guid Id,string Firstname, string Lastname, string PhoneNumber, string Email);