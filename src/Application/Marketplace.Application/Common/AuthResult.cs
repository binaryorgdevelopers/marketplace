using Marketplace.Application.Common.Messages;
using Marketplace.Application.Common.Messages.Messages;

namespace Marketplace.Application.Common;

public record AuthResult(SignedUp User, JsonWebToken Token);