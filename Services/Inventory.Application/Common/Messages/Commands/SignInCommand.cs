using Shared.Abstraction.Messaging;
using Shared.Models;

namespace Marketplace.Application.Common.Messages.Commands;

public record SellerSignInCommand(string Email, string Password) : ICommand<AuthResult>;