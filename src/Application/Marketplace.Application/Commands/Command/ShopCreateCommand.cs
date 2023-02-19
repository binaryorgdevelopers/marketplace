using Marketplace.Application.Commands.ICommand;
using Marketplace.Application.Common;
using Marketplace.Application.Common.Messages.Commands;
using Marketplace.Application.Common.Messages.Messages;
using Marketplace.Domain.Abstractions.Repositories;
using Marketplace.Domain.Entities;

namespace Marketplace.Application.Commands.Command;

public class ShopCreateCommand : IShopCreateCommand
{
    private readonly IGenericRepository<Shop> _shopRepository;
    private readonly IGenericRepository<User> _userRepository;

    public ShopCreateCommand(
        IGenericRepository<Shop> shopRepository,
        IGenericRepository<User> userRepository)
    {
        _shopRepository = shopRepository;
        _userRepository = userRepository;
    }

    public async Task<Either<ShopCreated, Exception>> CreateShop(ShopCreate shopCreate)
    {
        Shop shop = new Shop(shopCreate.Name, shopCreate.Number, shopCreate.Extras, shopCreate.UserId);
        await _shopRepository.AddAsync(shop);
        ShopCreated shopCreated = new ShopCreated(shop.UserId, shop.Name, shop.Number, shop.Extras);

        return new Either<ShopCreated, Exception>(shopCreated);
    }
}