using Marketplace.Application.Common;
using Marketplace.Application.Common.Messages.Commands;
using Marketplace.Application.Common.Messages.Messages;

namespace Marketplace.Application.Commands.ICommand;

public interface IShopCreateCommand
{
    Task<Either<ShopCreated,Exception>> CreateShop(ShopCreate shopCreate);
}