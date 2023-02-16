using Marketplace.Application.Common.Messages.Messages;
using Marketplace.Domain.Entities;

namespace Marketplace.Application.Common;

public record AuthResult(SignedUp user, string Token);