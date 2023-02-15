using Marketplace.Domain.Entities;

namespace Marketplace.Domain.Repositories;

public interface IUserRepository
{
    Task<User> GetAsync(Func<bool, User> predicate);
    Task<User> AddAsync(User user);
    Task<User> UpdateUser(User user);


    Task<bool> RemoveUser(User user);
}