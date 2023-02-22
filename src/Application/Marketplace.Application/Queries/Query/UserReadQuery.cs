using Marketplace.Application.Common;
using Marketplace.Application.Common.Extensions;
using Marketplace.Application.Common.Messages.Messages;
using Marketplace.Application.Queries.IQuery;
using Marketplace.Domain.Abstractions.Repositories;
using Marketplace.Domain.Entities;

namespace Marketplace.Application.Queries.Query;

public class UserReadQuery : IUserReadQuery
{
    private readonly IGenericRepository<User?> _genericRepository;

    public UserReadQuery(IGenericRepository<User?> genericRepository)
    {
        _genericRepository = genericRepository;
    }

    public async Task<Either<IEnumerable<UserRead>, Exception>> AllUsers()
    {
        var user =await _genericRepository.GetWithInclude(c => c.Shops,
            c => new UserRead(c.Id, c.CreatedAt, c.UpdatedAt, c.Role.RoleToString(), c.FirstName, c.LastName,
                c.PhoneNumber, c.Email,
                c.Shops.Select(x=> new ShopRead(x.Id, x.Name, x.Number, x.Extras,
                    x.Files.Select(b => new BlobRead(b.Id, b.CreatedAt, b.UpdatedAt, b.FileName, b.Extras))))));
        return new Either<IEnumerable<UserRead>, Exception>(user);
    }

    public User? GetUserById(Guid? id) => _genericRepository.Get(c => c.Id == id);
}