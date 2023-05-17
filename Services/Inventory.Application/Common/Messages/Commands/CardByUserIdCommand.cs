using Marketplace.Application.Abstractions.Messaging;
using Marketplace.Application.Common.Messages.Messages;

namespace Marketplace.Application.Common.Messages.Commands;

public record CardByUserIdCommand(Guid UserId) : ICommand<CardByByUserIdDto>;

public record CardByByUserIdDto(Guid UserId, List<CardDetailsDto> cards);