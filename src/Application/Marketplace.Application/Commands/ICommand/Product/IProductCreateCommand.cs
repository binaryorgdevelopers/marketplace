using Marketplace.Application.Common;
using Marketplace.Application.Common.Messages.Commands;
using Marketplace.Application.Common.Messages.Messages;

namespace Marketplace.Application.Commands.ICommand.Product;

public interface IProductCreateCommand
{
    Task<Either<ProductRead, Exception>> ProductCreate(ProductCreate productCreate);
}