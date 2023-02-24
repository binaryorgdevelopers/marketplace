using Marketplace.Application.Common;
using Marketplace.Application.Common.Messages.Messages;
using Marketplace.Application.Queries.IQuery.Auth;
using Marketplace.Domain.Abstractions.Repositories;
using Marketplace.Domain.Entities;

namespace Marketplace.Application.Queries.Query.Auth;

public class UserReadQuery : IUserReadQuery
{
    private readonly IGenericRepository<User?> _genericRepository;

    public UserReadQuery(IGenericRepository<User?> genericRepository)
    {
        _genericRepository = genericRepository;
    }

    public Either<IEnumerable<UserRead>,Exception> AllUsers()
    {
        throw new NotImplementedException();
    }

    public User? GetUserById(Guid? id) => _genericRepository.Get(c => c.Id == id);
}