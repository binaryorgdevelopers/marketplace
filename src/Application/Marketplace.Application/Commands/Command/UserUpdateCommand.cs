using Marketplace.Application.Commands.ICommand;
using Marketplace.Application.Common;
using Marketplace.Application.Common.Messages.Commands;
using Marketplace.Application.Common.Messages.Messages;
using Marketplace.Domain.Abstractions.Repositories;
using Marketplace.Domain.Constants;
using Marketplace.Domain.Entities;
using Marketplace.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace Marketplace.Application.Commands.Command;

public class UserUpdateCommand : IUserUpdateCommand
{
    private readonly IGenericRepository<User?> _repository;
    private readonly IPasswordHasher<User> _passwordHasher;

    public UserUpdateCommand(IGenericRepository<User?> repository, IPasswordHasher<User> passwordHasher)
    {
        _repository = repository;
        _passwordHasher = passwordHasher;
    }

    public async Task<Either<UserUpdated, AuthException>> UpdateUser(UpdateUser updateUser)
    {
        var user = await _repository.GetAsync(c => c.Email == updateUser.Email);

        if (user is null)
            return new Either<UserUpdated, AuthException>(new AuthException(Codes.InvalidCredential,
                $"User with Email :'{updateUser.Email}'not found."));

        user.SetPassword(updateUser.Password, _passwordHasher);
        _repository.Update(user);

        var userUpdated = new UserUpdated(user.Id, user.CreatedAt, user.UpdatedAt, nameof(user.Role),
            user.FirstName,
            user.PhoneNumber, user.Email);

        return new Either<UserUpdated, AuthException>(userUpdated);
    }
}