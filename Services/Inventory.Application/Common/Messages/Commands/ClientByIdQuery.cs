using Inventory.Domain.Entities;
using Marketplace.Application.Abstractions.Messaging;

namespace Marketplace.Application.Common.Messages.Commands;

public  record ClientByIdQuery(Guid? Id): ICommand<Clients>;