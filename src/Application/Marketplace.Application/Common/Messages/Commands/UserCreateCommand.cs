using Marketplace.Application.Abstractions.Messaging;

namespace Marketplace.Application.Common.Messages.Commands;

public record UserCreateCommand(string PhoneNumber, string Email, string Password, string Firstname, string Lastname,
    string Role) : ICommand<AuthResult>;