using Marketplace.Application.Abstractions.Messaging;

namespace Marketplace.Application.Common.Messages.Commands;

public record CustomerSignInCommand(string Email, string Password) : ICommand<AuthResult>;