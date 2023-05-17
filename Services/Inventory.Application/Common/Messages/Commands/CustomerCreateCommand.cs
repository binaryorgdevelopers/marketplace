using Marketplace.Application.Abstractions.Messaging;

namespace Marketplace.Application.Common.Messages.Commands;

public record CustomerCreateCommand(
    string FirstName,
    string LastName,
    string Username,
    string Password,
    string PhoneNumber,
    string Email) : ICommand<AuthResult>;