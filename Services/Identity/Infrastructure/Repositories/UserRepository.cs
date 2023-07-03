using System.Linq.Expressions;
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

    public async ValueTask<User?> GetUserAsync(Expression<Func<User, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public User? UpdateUser(User user)
    {
        var userToFind = _context.Users.FirstOrDefault(c => c.Id == user.Id);
        if (userToFind is null) throw new AuthException(Codes.UserNotFound, $"User with Id: '{user.Id}' not found");
        var userToUpdate = _context.Users.Update(user);

        _context.SaveChanges();
        return userToUpdate.Entity;
    }

    public async Task<User?> AddAsync(UserCreateCommand user, CancellationToken cancellationToken = default)
    {
        var userToCreate = new User(user.Firstname, user.Lastname, user.Email, user.PhoneNumber,user.RoleId.Value);
        userToCreate.SetPassword(user.Password, _passwordHasher);
        userToCreate.Activate();

        var result = await _context.Users.AddAsync(userToCreate, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return await GetUserAsync(c => c.Email == result.Entity.Email, cancellationToken);
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

    public async ValueTask<bool> ExistsAsync(Expression<Func<User, bool>> predicate,
        CancellationToken cancellationToken = default)
        => await _context.Users.AnyAsync(predicate, cancellationToken: cancellationToken);
}

public interface IUserRepository
{
    public ValueTask<User?> GetUserAsync(Expression<Func<User, bool>> predicate,
        CancellationToken cancellationToken = default);

    public User? UpdateUser(User user);
    public Task<User?> AddAsync(UserCreateCommand user, CancellationToken cancellationToken = default);
    public ValueTask<bool> DeactivateUser(User user, CancellationToken cancellationToken = default);

    public ValueTask<bool> ExistsAsync(Expression<Func<User, bool>> predicate,
        CancellationToken cancellationToken = default);
}