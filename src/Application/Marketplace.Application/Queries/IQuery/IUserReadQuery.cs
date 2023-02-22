using Marketplace.Application.Common;
using Marketplace.Application.Common.Messages.Messages;
using Marketplace.Domain.Entities;

namespace Marketplace.Application.Queries.IQuery;

public interface IUserReadQuery
{
    Task<Either<IEnumerable<UserRead>, Exception>> AllUsers();
    User? GetUserById(Guid? Id);
}