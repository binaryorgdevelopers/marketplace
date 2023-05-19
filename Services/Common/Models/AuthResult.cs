using Shared.Messages;

namespace Shared.Models;

public record AuthResult(Authorized User, JsonWebToken Token);