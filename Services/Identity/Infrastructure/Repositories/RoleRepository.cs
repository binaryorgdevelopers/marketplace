using System.Linq.Expressions;
using Authentication;
using Identity.Domain.Entities;
using Identity.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Shared.Models.Constants;

namespace Identity.Infrastructure.Repositories;

internal class RoleRepository : IRoleRepository
{
    private readonly IdentityContext _identityContext;

    public RoleRepository(IdentityContext identityContext)
    {
        _identityContext = identityContext;
    }

    public Task<Role?> GetRoleAsync(Expression<Func<Role, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return _identityContext.Roles.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public Role? UpdateRole(Role role, CancellationToken cancellationToken = default)
    {
        var roleToFind = _identityContext.Roles.FirstOrDefault(c => c.Id == role.Id || c.Name == role.Name);
        if (roleToFind is null) throw new AuthException(Codes.InvalidRole, "Roles with givenx credentials not found");
        var roleToUpdate = _identityContext.Roles.Update(role);
        _identityContext.SaveChanges();
        return roleToUpdate.Entity;
    }

    public async Task<Role?> AddRoleAsync(Role role, CancellationToken cancellationToken = default)
    {
        var entityEntry = await _identityContext.Roles.AddAsync(role, cancellationToken);
        await _identityContext.SaveChangesAsync(cancellationToken);
        return entityEntry.Entity;
    }

    public ValueTask<bool> DeactivateRole(Role user, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async ValueTask<bool> ExistsAsync(Expression<Func<Role, bool>> predicate,
        CancellationToken cancellationToken = default) =>
        await _identityContext.Roles.AnyAsync(predicate, cancellationToken: cancellationToken);
}

public interface IRoleRepository
{
    public Task<Role?> GetRoleAsync(Expression<Func<Role, bool>> predicate,
        CancellationToken cancellationToken = default);

    public Role? UpdateRole(Role user, CancellationToken cancellationToken = default);
    public Task<Role?> AddRoleAsync(Role user, CancellationToken cancellationToken = default);
    public ValueTask<bool> DeactivateRole(Role user, CancellationToken cancellationToken = default);

    public ValueTask<bool> ExistsAsync(Expression<Func<Role, bool>> predicate,
        CancellationToken cancellationToken = default);
}