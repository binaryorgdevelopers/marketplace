using Inventory.Domain.Abstractions.Repositories;
using Inventory.Domain.Entities;
using Marketplace.Application.Common.Messages.Commands;
using Shared.Abstraction.Messaging;
using Shared.Models;
using Shared.Models.Constants;

namespace Marketplace.Application.Commands.Command.UserCommands;

public class UserBlockCommandHandler : ICommandHandler<UserBlockCommand>
{
    private readonly IGenericRepository<Customer> _repository;

    public UserBlockCommandHandler(IGenericRepository<Customer> repository)
    {
        _repository = repository;
    }


    public async Task<Result> Handle(UserBlockCommand request, CancellationToken cancellationToken)
    {
        var findCustomer = await _repository.GetAsync(c => c.Id == request.UserId);
        if (findCustomer is null)
            return Result.Failure(new Error(Codes.InvalidCredential, $"Customer with Id:'{request.UserId}' not found"));
        findCustomer.ChangeStatus();
        _repository.Update(findCustomer);
        return Result.Success($"User with Id:'{findCustomer.Id}' blocked!");
    }
}