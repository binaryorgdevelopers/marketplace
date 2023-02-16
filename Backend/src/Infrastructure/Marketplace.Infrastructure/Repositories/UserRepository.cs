using Marketplace.Application.Common.Interface.Database;
using Marketplace.Domain.Entities;
using Marketplace.Domain.Repositories;

namespace Marketplace.Infrastructure.Repositories;
public class UserRepository : IUserRepository
{
    private readonly IGenericRepository<User> _repository;

    public UserRepository(IGenericRepository<User> repository)
    {
        _repository = repository;
    }

    public async Task<User?> GetAsync(Guid id)
        => await _repository.GetAsync(id);

    public async Task<User?> GetAsync(string email)
        => await _repository.GetAsync(x => x.Email == email.ToLowerInvariant());

    public Task AddAsync(User user)
        => _repository.AddAsync(user);

    public void UpdateAsync(User user)
        => _repository.Update(user);
}