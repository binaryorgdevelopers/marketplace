using Marketplace.Application.Abstractions.Messaging;
using Marketplace.Application.Common.Messages.Commands;
using Inventory.Domain.Abstractions.Repositories;
using Inventory.Domain.Entities;
using Inventory.Domain.Shared;

namespace Marketplace.Application.Queries.Query.Client;

public class ClientByIdQueryHandler : ICommandHandler<ClientByIdQuery,Clients>
{
    private readonly IGenericRepository<Clients> _clientRepository;

    public ClientByIdQueryHandler(IGenericRepository<Clients> clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<Result<Clients>> Handle(ClientByIdQuery request, CancellationToken cancellationToken)
    {
        var client = await _clientRepository.GetAsync(c => c.Id == request.Id);
        return Result.Success(client);
    }
}