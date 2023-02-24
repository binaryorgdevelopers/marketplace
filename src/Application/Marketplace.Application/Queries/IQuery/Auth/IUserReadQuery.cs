using Marketplace.Application.Common;
using Marketplace.Application.Common.Messages.Messages;
using Marketplace.Domain.Entities;

namespace Marketplace.Application.Queries.IQuery.Auth;

public interface IUserReadQuery
{
    Either<IEnumerable<UserRead>, Exception> AllUsers();
    User? GetUserById(Guid? Id);
}