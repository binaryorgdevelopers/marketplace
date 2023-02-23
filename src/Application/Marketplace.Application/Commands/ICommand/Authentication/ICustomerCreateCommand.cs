using Marketplace.Application.Common;
using Marketplace.Application.Common.Messages.Commands;

namespace Marketplace.Application.Commands.ICommand.Authentication;

public interface ICustomerCreateCommand
{
    Task<Either<AuthResult, Exception>> CreateCustomer(CustomerCreate customerCreate);
}