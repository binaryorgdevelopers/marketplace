using Marketplace.Application.Common;
using Marketplace.Application.Common.Messages.Messages;
using Marketplace.Application.Queries.IQuery;
using Marketplace.Domain.Abstractions.Repositories;
using Marketplace.Domain.Constants;
using Marketplace.Domain.Entities;

namespace Marketplace.Application.Queries.Query;

public class UserReadQuery : IUserReadQuery
{
    private readonly IGenericRepository<User?> _genericRepository;

    public UserReadQuery(IGenericRepository<User?> genericRepository)
    {
        _genericRepository = genericRepository;
    }

    public Either<IEnumerable<UserRead>, Exception> AllUsers()
    {
        var users = _genericRepository.GetAll(c => c.Shops);
        // IEnumerable<UserRead> userRead = users.Select(c
        //     => new UserRead(
        //         c.Id,
        //         c.CreatedAt,
        //         c.UpdatedAt,
        //         c.Role.RoleToString(),
        //         c.FirstName,
        //         c.LastName,
        //         c.PhoneNumber,
        //         c.Email,
        //         c.Shops));
        var user2 = _genericRepository.GetWithInclude(c => c.Shops,
            c => new UserRead(c.Id, c.CreatedAt, c.UpdatedAt, c.Role.RoleToString(), c.FirstName, c.LastName, c.PhoneNumber, c.Email,c.Shops));
        return new Either<IEnumerable<UserRead>, Exception>(user2);
    }

    public User? GetUserById(Guid? Id) => _genericRepository.Get(c => c.Id == Id);
}