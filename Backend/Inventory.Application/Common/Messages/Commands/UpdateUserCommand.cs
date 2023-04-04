using Marketplace.Application.Abstractions.Messaging;

namespace Marketplace.Application.Common.Messages.Commands;

public record UpdateUserCommand(string Email, string Password, string FirstName, string LastName) : ICommand;