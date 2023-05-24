using Identity.Domain.Entities;
using Identity.Infrastructure.Repositories;
using Identity.Models;
using Shared.Models;
using Shared.Models.Constants;

namespace Identity.Infrastructure.Services;

public class RoleManagerService
{
    private readonly IRoleRepository _repository;

    public RoleManagerService(IRoleRepository repository)
    {
        _repository = repository;
    }

    public async ValueTask<Result<Role>> CreateRole(RoleCreateCommand createCommand)
    {
        if (await _repository.ExistsAsync(c => c.Name == createCommand.Name))
            return Result.Failure<Role>(new Error(Codes.InvalidCredential, "Roles already exist"));
        
        Role role = new Role(createCommand.Name);
        var saveResult = await _repository.AddRoleAsync(role);
        return Result.Success(saveResult);
    }
}