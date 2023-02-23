using Marketplace.Application.Common;
using Marketplace.Application.Common.Messages.Commands;

namespace Marketplace.Application.Commands.ICommand.Authentication;

public interface ISellerCreateCommand
{
    Task<Either<AuthResult,Exception>> CreateSeller(SellerCreate sellerCreate);
}