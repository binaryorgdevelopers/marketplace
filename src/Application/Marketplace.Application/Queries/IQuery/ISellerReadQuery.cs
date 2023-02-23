using Marketplace.Application.Common;
using Marketplace.Application.Common.Messages.Commands;

namespace Marketplace.Application.Queries.IQuery;

public interface ISellerReadQuery
{
    Either<AuthResult, Exception> SellerSignIn(SignInCommand signInCommand);
}