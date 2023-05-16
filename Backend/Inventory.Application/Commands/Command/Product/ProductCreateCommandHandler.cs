using Inventory.Domain.Abstractions.Repositories;
using Inventory.Domain.Abstractions.Services;
using Inventory.Domain.Entities;
using Inventory.Domain.Shared;
using Marketplace.Application.Abstractions.Messaging;
using Marketplace.Application.Common.Messages.Commands;
using Marketplace.Application.Common.Messages.Messages;

namespace Marketplace.Application.Commands.Command.Product;

public class ProductCreateCommandHandler : ICommandHandler<ProductCreateCommand, ProductDto>
{
    private readonly IGenericRepository<Inventory.Domain.Entities.Product> _productRepository;

    private readonly ICloudUploaderService _uploaderService;

    public ProductCreateCommandHandler(IGenericRepository<Inventory.Domain.Entities.Product> productRepository,
        ICloudUploaderService uploaderService)
    {
        _productRepository = productRepository;
        _uploaderService = uploaderService;
    }

    #region ProductCreate Handler

    public async Task<Result<ProductDto>> Handle(ProductCreateCommand request, CancellationToken cancellationToken)
    {
        List<Blob> blob = new();
        foreach (var b in request.Photos)
        {
            blob.Add(Blob.Create(await _uploaderService.Upload(b.file, string.Empty), b.Extras));
        }

        var product = Inventory.Domain.Entities.Product.Create(
            request.Title,
            request.Price,
            request.Count,
            request.Description,
            request.CategoryId,
            request.SellerId,
            blob,
            request.Characteristics.Select(c => c.ToChars()),
            request.Badges.Select(c => c.ToBadge()));

        await _productRepository.AddAsync(product);

        return Result.Success(new ProductDto(product.Id, product.Attributes, product.Badges,
            product.Synonyms, product.Title, product.Description, CategoryDto.FromEntity(product.Category),
            SellerDto.FromEntity(product.Seller), product.Photos, product.Characteristics));
    }

    #endregion
}