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
        // var commandHandlerTypes = AppDomain.CurrentDomain.GetAssemblies()
        //     .SelectMany(x => x.GetTypes())
        //     .Where(x => x.GetInterfaces()
        //         .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<>)))
        //     .ToList();
        //
        // var handlerTypeMap = new Dictionary<string, Type>();
        // foreach (var commandHandlerType in commandHandlerTypes)
        // {
        //     var interfaceType = commandHandlerType.GetInterfaces()
        //         .Single(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<>));
        //     var commandType = interfaceType.GetGenericArguments().Single();
        //     var handlerTypeName = commandType.Name + "Handler";
        //     handlerTypeMap[handlerTypeName] = commandHandlerType;
        // }
        //
        // if (handlerTypeMap.TryGetValue(request.HandlerType, out var handlerType))
        // {
        //     var handler = Activator.CreateInstance(handlerType);
        //     var method = handler.GetType().GetMethod("HandleAsync");
        //     await (Task)method.Invoke(handler, new object[] { request });
        // }
        //
        // return Result.Create("Unknown Handler");

        var findCustomer = await _repository.GetAsync(c => c.Id == request.UserId);
        if (findCustomer is null)
            return Result.Failure(new Error(Codes.InvalidCredential, $"Customer with Id:'{request.UserId}' not found"));
        findCustomer.ChangeStatus();
        _repository.Update(findCustomer);
        return Result.Success($"User with Id:'{findCustomer.Id}' blocked!");
    }
}