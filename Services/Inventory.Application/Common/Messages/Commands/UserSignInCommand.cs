using Shared.Abstraction.Messaging;
using Shared.Models;

namespace Marketplace.Application.Common.Messages.Commands;

public record UserSignInCommand(string Email, string Password) : ICommand<AuthResult>;