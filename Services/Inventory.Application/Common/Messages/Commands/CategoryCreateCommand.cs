using Shared.Abstraction.Messaging;

namespace Marketplace.Application.Common.Messages.Commands;

public record CategoryCreateCommand(
    Guid Id,
    string Title,
    Guid? ParentId
) : ICommand;