namespace Marketplace.Application.Common.Messages.Commands;

public record CustomerCreate(
    string FirstName, 
    string LastName,
    string Username,
    string Password,
    string PhoneNumber,
    string Email);