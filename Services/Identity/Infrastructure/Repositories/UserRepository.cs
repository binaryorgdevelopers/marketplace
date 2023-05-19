using Authentication;
using Identity.Domain.Entities;
using Identity.Infrastructure.Persistence;
using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Models.Constants;

namespace Identity.Infrastructure.Repositories;

internal class UserRepository : IUserRepository
{
    private readonly IdentityContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;

    public UserRepository(IdentityContext context, IPasswordHasher<User> passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async ValueTask<User?> GetUserAsync(Func<User, bool> predicate,
        CancellationToken cancellationToken = default)
        => await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(c => c.Id == Guid.Empty, cancellationToken);

    public User? UpdateUser(User user)
    {
        var userToFind = _context.Users.FirstOrDefault(c => c.Id == user.Id);
        if (userToFind is null) throw new AuthException(Codes.UserNotFound, $"User with Id: '{user.Id}' not found");
        var userToUpdate = _context.Users.Update(user);
        return userToUpdate.Entity;
    }

    public async Task<User?> AddAsync(UserCreateCommand user, CancellationToken cancellationToken = default)
    {
        var isExists = _context.Users.Any(c => c.Email == user.Email);
        if (isExists)
            throw new AuthException(Codes.InvalidCredential, $"User with given email:''{user.Email} not found");

        User userToCreate = new User(user.Firstname, user.Lastname, user.Email, user.PhoneNumber);
        userToCreate.SetPassword(user.Password, _passwordHasher);
        userToCreate.Activate();

        var result = await _context.Users.AddAsync(userToCreate, cancellationToken);
        
        return result.Entity;
    }

    public async ValueTask<bool> DeactivateUser(User user, CancellationToken cancellationToken = default)
    {
        var userToUpdate = new User
        {
            Id = user.Id,
            IsActive = false
        };
        _context.Users.Attach(user);
        _context.Entry(userToUpdate).State = EntityState.Modified;
        var updateCount = await _context.SaveChangesAsync(cancellationToken);
        return updateCount > 0;
    }
}

public interface IUserRepository
{
    public ValueTask<User?> GetUserAsync(Func<User, bool> predicate, CancellationToken cancellationToken = default);
    public User? UpdateUser(User user);
    public Task<User?> AddAsync(UserCreateCommand user, CancellationToken cancellationToken = default);
    public ValueTask<bool> DeactivateUser(User user, CancellationToken cancellationToken = default);
}