using Marketplace.Application.Common;
using Marketplace.Application.Common.Messages.Commands;
using Marketplace.Domain.Exceptions;

namespace Marketplace.Application.Commands;

public interface IUserCreateCommand
{
    Task<Either<AuthResult, AuthException>> CreateUser(UserCreate user);
}