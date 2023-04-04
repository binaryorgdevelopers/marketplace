using Marketplace.Application.Abstractions.Messaging;

namespace Marketplace.Application.Common.Messages.Commands;

public record SellerSignInCommand(string Email, string Password) : ICommand<AuthResult>;