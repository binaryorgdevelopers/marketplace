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
        => _identityContext.Roles.FirstOrDefaultAsync(predicate, cancellationToken);

    public Role? UpdateRole(Role role, CancellationToken cancellationToken = default)
    {
        var roleToFind = _identityContext.Roles.FirstOrDefault(c => c.Id == role.Id || c.Name == role.Name);
        if (roleToFind is null) throw new AuthException(Codes.InvalidRole, $"Role with given credentials not found");
        var roleToUpdate = _identityContext.Roles.Update(role);
        return roleToUpdate.Entity;
    }

    public Task<Role?> AddRoleAsync(Role user, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public ValueTask<bool> DeactivateRole(Role user, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}

internal interface IRoleRepository
{
    public Task<Role?> GetRoleAsync(Expression<Func<Role, bool>> predicate,
        CancellationToken cancellationToken = default);

    public Role? UpdateRole(Role user, CancellationToken cancellationToken = default);
    public Task<Role?> AddRoleAsync(Role user, CancellationToken cancellationToken = default);
    public ValueTask<bool> DeactivateRole(Role user, CancellationToken cancellationToken = default);
}