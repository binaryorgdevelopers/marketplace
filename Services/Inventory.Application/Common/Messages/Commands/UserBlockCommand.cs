using Marketplace.Application.Abstractions.Messaging;

namespace Marketplace.Application.Common.Messages.Commands;

public record UserBlockCommand(Guid UserId) : ICommand;