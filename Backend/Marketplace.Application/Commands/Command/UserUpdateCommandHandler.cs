using Marketplace.Application.Abstractions.Messaging;
using Marketplace.Application.Common.Messages.Commands;
using Marketplace.Application.Common.Messages.Events;
using Marketplace.Domain.Abstractions.Repositories;
using Marketplace.Domain.Entities;
using Marketplace.Domain.Models.Constants;
using Marketplace.Domain.Shared;
using Microsoft.AspNetCore.Identity;

namespace Marketplace.Application.Commands.Command;

public class UserUpdateCommand : ICommandHandler<UpdateUserCommand>
{
    private readonly IGenericRepository<User?> _repository;
    private readonly IPasswordHasher<User> _passwordHasher;

    public UserUpdateCommand(IGenericRepository<User?> repository, IPasswordHasher<User> passwordHasher)
    {
        _repository = repository;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
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