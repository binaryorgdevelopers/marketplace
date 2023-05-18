using Inventory.Domain.Abstractions.Repositories;
using Inventory.Domain.Entities;
using Inventory.Domain.Models.Constants;
using Inventory.Domain.Shared;
using Marketplace.Application.Abstractions.Messaging;
using Marketplace.Application.Common.Messages.Commands;
using Marketplace.Application.Common.Messages.Messages;

namespace Marketplace.Application.Queries.Query.UserQueries;

public class CardByUserIdQueryHandler : ICommandHandler<CardByUserIdCommand, CardByByUserIdDto>
{
    private readonly IGenericRepository<CardDetail> _cardRepository;

    public CardByUserIdQueryHandler(IGenericRepository<CardDetail> cardRepository)
    {
        _cardRepository = cardRepository;
    }

    public Task<Result<CardByByUserIdDto>> Handle(CardByUserIdCommand request,
        CancellationToken cancellationToken) =>
        Task.Run(() =>
        {
            var cards = _cardRepository.Get(c => c.CustomerId == request.UserId);
            if (cards is null)
                return Result.Failure<CardByByUserIdDto>(new Error(Codes.InvalidCredential, "User doesn't have cards"));
            return Result.Success(new CardByByUserIdDto(request.UserId,
                cards.Select(c => CardDetailsDto.FromCardDetails(c)).ToList()));
        }, cancellationToken);
}