using Marketplace.Application.Common;
using Marketplace.Application.Common.Messages.Messages;

namespace Marketplace.Application.Queries.IQuery;

public interface IUserReadQuery
{
    Either<IEnumerable<UserRead>, Exception> AllUsers();
}