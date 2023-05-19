using Shared.Abstraction.Messaging;
using Shared.Models;

namespace Marketplace.Application.Common.Messages.Commands;

public record CustomerSignInCommand(string Email, string Password) : ICommand<AuthResult>;