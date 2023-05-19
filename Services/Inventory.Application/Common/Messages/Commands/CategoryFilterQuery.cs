using Marketplace.Application.Common.Messages.Messages;
using Shared.Abstraction.Messaging;

namespace Marketplace.Application.Common.Messages.Commands;

public record CategoryFilterQuery(string field,string value) : ICommand<CategoryDto>;