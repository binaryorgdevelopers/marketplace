using Marketplace.Application.Common;
using Marketplace.Application.Common.Messages.Commands;

namespace Marketplace.Application.Queries.IQuery.Auth;

public interface ICustomerReadQuery
{
    Either<AuthResult,Exception> SignIn(SignInCommand signInCommand);
}