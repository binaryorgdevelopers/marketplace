using Marketplace.Application.Abstractions.Messaging;
using Marketplace.Domain.Entities;

namespace Marketplace.Application.Common.Messages.Commands;

public  record ClientByIdQuery(Guid? Id): ICommand<Clients>;