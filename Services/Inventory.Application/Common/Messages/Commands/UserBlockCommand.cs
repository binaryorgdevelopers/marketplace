using Shared.Abstraction.Messaging;

namespace Marketplace.Application.Common.Messages.Commands;

public record UserBlockCommand(Guid UserId) : ICommand;