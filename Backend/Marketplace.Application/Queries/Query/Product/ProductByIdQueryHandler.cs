using Marketplace.Application.Abstractions.Messaging;
using Marketplace.Application.Common.Messages.Messages;
using Marketplace.Domain.Abstractions.Repositories;
using Marketplace.Domain.Models.Constants;
using Marketplace.Domain.Shared;

namespace Marketplace.Application.Queries.Query.Product;

public class ProductByIdQueryHandler : ICommandHandler<ProductByIdQuery>
{
    private readonly IGenericRepository<Domain.Entities.Product> _productRepository;

    public ProductByIdQueryHandler(IGenericRepository<Domain.Entities.Product> productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result> Handle(ProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = _productRepository.GetWithSelect<ProductDto>(
            c => c.Id == request.Id,
            c => ProductDto.FromEntity(c),
            i => i.Characteristics,
            i => i.Category,
            i => i.Badges,
            i => i.Photos);

        return product is null
            ? Result.Failure(new Error(Codes.InvalidCredential, $"Product with Id:'{request.Id} not found'"))
            : Result.Success(product);
    }
}

public sealed record ProductByIdQuery(Guid Id) : ICommand;