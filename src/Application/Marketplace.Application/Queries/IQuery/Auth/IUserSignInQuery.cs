using Marketplace.Application.Common;
using Marketplace.Application.Common.Messages.Commands;
using Marketplace.Domain.Exceptions;

namespace Marketplace.Application.Queries.IQuery.Auth;

public interface IUserSignInQuery
{
    Task<Either<AuthResult, AuthException>> SignIn(SignInCommand signInCommand);
}