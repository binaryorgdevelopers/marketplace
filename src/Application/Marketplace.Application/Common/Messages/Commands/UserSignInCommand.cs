using Marketplace.Application.Abstractions.Messaging;

namespace Marketplace.Application.Common.Messages.Commands;

public record UserSignInCommand(string Email, string Password) : ICommand<AuthResult>;