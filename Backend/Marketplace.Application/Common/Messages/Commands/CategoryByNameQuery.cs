using Marketplace.Application.Abstractions.Messaging;
using Marketplace.Application.Common.Messages.Messages;

namespace Marketplace.Application.Common.Messages.Commands;

public record CategoryByNameQuery(string Title) : ICommand<CategoryRead>;