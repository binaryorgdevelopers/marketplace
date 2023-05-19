using Inventory.Domain.Entities;
using Shared.Abstraction.Messaging;

namespace Marketplace.Application.Common.Messages.Commands;

public  record ClientByIdQuery(Guid? Id): ICommand<Clients>;