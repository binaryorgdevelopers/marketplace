﻿using Marketplace.Application.Common.Messages.Messages;
using Shared.Abstraction.Messaging;

namespace Marketplace.Application.Common.Messages.Commands;

public record ProductCreateCommand(
    Guid UserId,
    Guid CategoryId,
    string Title,
    decimal Price,
    int Count,
    string Description,
    IEnumerable<CharacteristicsCreate> Characteristics,
    IEnumerable<BadgeCreate> Badges,
    IEnumerable<BlobCreate> Photos) : ICommand<ProductDto>;