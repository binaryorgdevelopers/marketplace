using Identity.Domain.Entities;
using Identity.Infrastructure.Repositories;
using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Shared.Messages;
using Shared.Models;
using Shared.Models.Constants;

namespace Identity.Infrastructure.Services;

public class UserManagerService
{
    private readonly IUserRepository _repository;
    private readonly ITokenProvider _tokenProvider;
    private readonly IRoleRepository _roleRepository;
    private readonly IPasswordHasher<User> _passwordHasher;

    public UserManagerService(IUserRepository repository, ITokenProvider tokenProvider, IRoleRepository roleRepository,
        IPasswordHasher<User> passwordHasher)
    {
        _repository = repository;
        _tokenProvider = tokenProvider;
        _roleRepository = roleRepository;
        _passwordHasher = passwordHasher;
    }

    public async ValueTask<Result<AuthResult>> Register(UserCreateCommand createCommand)
    {
        if (await _repository.ExistsAsync(u => u.Email == createCommand.Email))
            return Result.Failure<AuthResult>(
                new Error(Codes.InvalidCredential, $"User with email:''{createCommand.Email} already exists"));

        var role = await _roleRepository.GetRoleAsync(r => r.Name == createCommand.RoleName);

        if (role is null)
            return Result.Failure<AuthResult>(new Error(Codes.InvalidRole, "Invalid Roles"));
        var newUser = createCommand with { RoleId = role.Id };


        var saveResult = await _repository.AddAsync(newUser, new CancellationTokenSource().Token);

        if (saveResult is null)
            return Result.Failure<AuthResult>(new Error(Codes.InvalidCredential, "Error while saving new User"));

        var token = await _tokenProvider.GenerateToken(saveResult.ToTokenRequest());

        return Result.Create(new AuthResult(new Authorized(saveResult.Id, saveResult.FirstName, saveResult.LastName,
            saveResult.PhoneNumber,
            saveResult.Email, saveResult.Role.Name), new JsonWebToken(token.Token, token.ExpiresIn)));
    }

    public async ValueTask<Result<AuthResult>> Login(UserSignInCommand command)
    {
        var user = await _repository.GetUserAsync(c => c.Email == command.Email);
        if (user is null)
            return Result.Failure<AuthResult>(new Error(Codes.InvalidCredential,
                $"User with email:'{command.Email} not found'"));

        if (!user.ValidatePassword(command.Password, _passwordHasher))
            return Result.Failure<AuthResult>(new Error(Codes.InvalidPassword, "Invalid password."));
        if (!user.IsActive)
            return Result.Failure<AuthResult>(
                new Error("user_blocked", "User blocked, Please contact customer support"));

        var token = await _tokenProvider.GenerateToken(user.ToTokenRequest());

        return Result.Success(new AuthResult(new Authorized(user.Id, user.FirstName, user.LastName, user.PhoneNumber,
            user.Email, user.Role.Name), token));
    }

    public async ValueTask<Result> ChangePassword(ChangePasswordCommand changePasswordCommand)
    {
        var user = await _repository.GetUserAsync(u => u.Email == changePasswordCommand.Email);

        if (user is null)
            return Result.Failure(new Error(Codes.UserNotFound,
                $"User with email:'{changePasswordCommand.Email}' not found"));

        if (!user.ValidatePassword(changePasswordCommand.OldPassword, _passwordHasher))
            return Result.Failure(new Error(Codes.InvalidPassword, "Wrong password"));

        user.SetPassword(changePasswordCommand.NewPassword, _passwordHasher);

        User? saveResult = _repository.UpdateUser(user);

        return saveResult is null
            ? Result.Failure(new Error(Codes.ServerError, "Error while saving entity, see inner exception"))
            : Result.Success("User password updated");
    }
}