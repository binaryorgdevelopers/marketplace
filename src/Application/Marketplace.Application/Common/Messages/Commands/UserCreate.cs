namespace Marketplace.Application.Common.Messages.Commands;

public record UserCreate(string PhoneNumber, string Email, string Password, string Firstname, string Lastname,
    string Role);