using Marketplace.Application.Abstractions.Messaging;
using Marketplace.Application.Common.Messages.Messages;

namespace Marketplace.Application.Common.Messages.Commands;

public record CategoryFilterQuery(string field,string value) : ICommand<CategoryDto>;