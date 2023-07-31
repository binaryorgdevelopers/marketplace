using Inventory.Domain.Abstractions.Repositories;
using Marketplace.Application.Common.Messages.Messages;
using Shared.Abstraction.Messaging;
using Shared.Models;
using Shared.Models.Constants;

namespace Marketplace.Application.Queries.Query.Product;

public class ProductByIdQueryHandler : ICommandHandler<ProductByIdQuery>
{
    private readonly IGenericRepository<Inventory.Domain.Entities.Product> _repository;
    private readonly IProductRepository _productRepository;

    public ProductByIdQueryHandler(IGenericRepository<Inventory.Domain.Entities.Product> repository,
        IProductRepository productRepository)
    {
        _repository = repository;
        _productRepository = productRepository;
    }

    public async Task<Result> Handle(ProductByIdQuery request, CancellationToken cancellationToken)
    {
        // var product = _repository.GetWithSelect<ProductDto>(
        //     c => c.Id == request.Id,
        //     c => ProductDto.FromEntity(c),
        //     i => i.Characteristics,
        //     i => i.Category,
        //     i => i.Badges,
        //     i => i.Photos);
        var product = await _productRepository.GetById(request.Id);
        var productDto = ProductDto.FromEntity(product);
        return productDto is null
            ? Result.Failure(new Error(Codes.InvalidCredential, $"Product with Id:'{request.Id} not found'"))
            : Result.Success(productDto);
    }
}

public sealed record ProductByIdQuery(Guid Id) : ICommand;