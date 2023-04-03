﻿using Marketplace.Application.Common.Messages.Events;
using Marketplace.Domain.Models;

namespace Marketplace.Application.Common;

public record AuthResult(Authorized User, JsonWebToken Token);