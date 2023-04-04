using Marketplace.Application.Common.Messages.Events;
using Inventory.Domain.Models;

namespace Marketplace.Application.Common;

public record AuthResult(Authorized User, JsonWebToken Token);