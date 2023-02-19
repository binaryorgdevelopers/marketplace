namespace Marketplace.Application.Common.Messages.Commands;

public record UpdateUser(string Email, string Password, string FirstName, string LastName);