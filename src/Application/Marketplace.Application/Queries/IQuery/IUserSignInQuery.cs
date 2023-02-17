using Marketplace.Application.Common;
using Marketplace.Application.Common.Messages.Commands;
using Marketplace.Domain.Exceptions;

namespace Marketplace.Application.Queries;

public interface IUserSignInQuery
{
    Task<Either<AuthResult, AuthException>> SignIn(SignIn signIn);
}