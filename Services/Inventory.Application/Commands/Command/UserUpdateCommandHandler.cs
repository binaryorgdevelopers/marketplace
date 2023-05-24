using Inventory.Domain.Abstractions.Repositories;
using Inventory.Domain.Entities;
using Marketplace.Application.Common.Messages.Commands;
using Marketplace.Application.Common.Messages.Events;
using Microsoft.AspNetCore.Identity;
using Shared.Abstraction.Messaging;
using Shared.Models;
using Shared.Models.Constants;

namespace Marketplace.Application.Commands.Command;

public class UserUpdateCommand : ICommandHandler<UpdateUserCommand>
{
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IGenericRepository<User?> _repository;

    public UserUpdateCommand(IGenericRepository<User?> repository, IPasswordHasher<User> passwordHasher)
    {
        _repository = repository;
        _passwordHasher = passwordHasher;
    }

    public async ValueTask<Result> HandleAsync(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetAsync(c => c.Email == request.Email);

        if (user is null)
            return Result.Failure(new Error(Codes.InvalidCredential,
                $"User with Email :'{request.Email}'not found."));

        user.SetPassword(request.Password, _passwordHasher);
        _repository.Update(user);

        var userUpdated = new UserUpdated(user.Id, user.CreatedAt, user.UpdatedAt, nameof(user.Role),
            user.FirstName,
            user.PhoneNumber, user.Email);

        return Result.Success(userUpdated);
    }
}