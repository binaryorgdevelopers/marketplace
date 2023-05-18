using Inventory.Domain.Abstractions.Repositories;
using Inventory.Domain.Entities;
using Inventory.Domain.Models.Constants;
using Inventory.Domain.Shared;
using Marketplace.Application.Abstractions.Messaging;
using Marketplace.Application.Common.Messages.Commands;

namespace Marketplace.Application.Commands.Command.UserCommands;

public class CardToUserCommandHandler : ICommandHandler<BindCardToUserCommand>
{
    private readonly IGenericRepository<CardDetail> _cardRepository;
    private readonly IGenericRepository<Customer> _customerRepository;

    public CardToUserCommandHandler(IGenericRepository<CardDetail> cardRepository,
        IGenericRepository<Customer> customerRepository)
    {
        _cardRepository = cardRepository;
        _customerRepository = customerRepository;
    }

    public async Task<Result> Handle(BindCardToUserCommand request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetAsync(c => c.Id == request.UserId, cancellationToken);
        if (customer is null)
            return Result.Failure(new Error(Codes.InvalidCredential,
                $"Customer with Id:'{request.UserId}' not found."));
        await _cardRepository.AddRangeAsync(request.ToCardDetails(), cancellationToken);
        await _cardRepository.SaveChangesAsync(cancellationToken);
        return Result.Success($"{request.Cards.Count} cards bound to user:'{request.UserId}.'");
    }
}