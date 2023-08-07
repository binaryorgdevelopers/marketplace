using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Authentication;
using Identity.Domain.Entities;
using Identity.Infrastructure.Persistence;
using Identity.Models;
using Identity.Models.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Models.Constants;


namespace Identity.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IdentityContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly ICacheRepository _cacheRepository;


    public UserRepository(IdentityContext context, IPasswordHasher<User> passwordHasher,
        ICacheRepository cacheRepository)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _cacheRepository = cacheRepository;
    }


    public async ValueTask<User?> GetUserByEmail(string email, CancellationToken cancellationToken = default)
    {
        var dbUser = await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(c => c.Email == email, cancellationToken);

        if (dbUser is not null) dbUser.Role.User = null;

        await _cacheRepository.SetStringAsync(email, dbUser!);
        return dbUser;
    }

    public async ValueTask<UserDto?> GetUserById(Guid id, CancellationToken cancellationToken = default)
    {
        var cacheUser = await _cacheRepository.GetStringAsync<UserDto>(id.ToString());
        if (cacheUser is not null) return cacheUser;

        var dbUser = await _context.Users
            .Include(u => u.Role)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        if (dbUser == null) return null;

        UserDto userDto = dbUser.ToDto();

        if (dbUser.Cards != null)
        {
            userDto = userDto with { Cards = dbUser.Cards.Select(c => c.ToDto()) };
        }

        await _cacheRepository.SetStringAsync(id.ToString(), userDto);

        return userDto;
    }

    public User UpdateUser(User user)
    {
        var userToFind = _context.Users.FirstOrDefault(c => c.Id == user.Id);
        if (userToFind is null) throw new AuthException(Codes.UserNotFound, $"User with Id: '{user.Id}' not found");
        
        _context.Entry(userToFind).State = EntityState.Detached;

        var updated = _context.Users.Update(user);
        _context.SaveChanges();
        return updated.Entity;
    }

    public async Task<User?> AddAsync(UserCreateCommand user, CancellationToken cancellationToken = default)
    {
        var userToCreate = new User(user.Firstname, user.Lastname, user.Email, user.PhoneNumber, user.RoleId.Value);
        userToCreate.SetPassword(user.Password, _passwordHasher);
        userToCreate.Activate();
        userToCreate.Cards = new List<CardDetail>();

        var result = await _context.Users.AddAsync(userToCreate, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
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

    public async ValueTask<bool> ExistsAsync(Expression<Func<User, bool>> predicate,
        CancellationToken cancellationToken = default)
        => await _context.Users.AnyAsync(predicate, cancellationToken: cancellationToken);

    public async ValueTask<User> GetUserWithCardsAsync(Expression<Func<User, bool>> expression,
        CancellationToken cancellationToken = default)
        => (await _context.Users
            .Where(expression)
            .Include(c => c.Cards)
            .FirstOrDefaultAsync(expression, cancellationToken))!;
}

public interface IUserRepository
{
    public ValueTask<User?> GetUserByEmail(string email, CancellationToken cancellationToken = default);
    public ValueTask<UserDto?> GetUserById(Guid id, CancellationToken cancellationToken = default);

    public User? UpdateUser(User user);
    public Task<User?> AddAsync(UserCreateCommand user, CancellationToken cancellationToken = default);
    public ValueTask<bool> DeactivateUser(User user, CancellationToken cancellationToken = default);

    public ValueTask<bool> ExistsAsync(Expression<Func<User, bool>> predicate,
        CancellationToken cancellationToken = default);

    public ValueTask<User> GetUserWithCardsAsync(Expression<Func<User, bool>> expression,
        CancellationToken cancellationToken = default);
}