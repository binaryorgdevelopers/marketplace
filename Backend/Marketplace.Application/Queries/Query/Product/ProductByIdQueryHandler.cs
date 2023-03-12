using Marketplace.Application.Abstractions.Messaging;
using Marketplace.Application.Common.Messages.Messages;
using Marketplace.Domain.Abstractions.Repositories;
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
        var product = await _productRepository.GetAsync(c => c.Id == request.Id);
        IEnumerable<BadgeDto> badges = product.Badges.Select(c =>
            new BadgeDto(c.Id, c.Text, c.TextColor, c.BackgroundColor, c.Description, c.Type));

        CategoryDto categories = new CategoryDto(product.Category.Id, product.Category.Title,
            product.Category.ProductAmount, ArraySegment<ProductDto>.Empty,null);
        SellerDto sellerDto = new SellerDto(product.Seller.Id, product.Seller.Title, product.Seller.Description,
            product.Seller.Info,
            product.Seller.Username, product.Seller.FirstName, product.Seller.LastName, product.Seller.Banner,
            product.Seller.Avatar, product.Seller.Link);
        IEnumerable<BlobDto> photos = product.Photos.Select(c => new BlobDto(c.Id, c.Title, c.Extras));
        IEnumerable<CharacteristicsRead> characteristics = product.Characteristics.Select(c =>
            new CharacteristicsRead(c.Id, c.Title, c.Values.Select(x => new ColorRead(x.Id, x.Title, x.Value))));

        return Result.Success(ProductDto.FromEntity(product));
    }
}

public sealed record ProductByIdQuery(Guid Id) : ICommand;

// https://t.me/shamsinurijodi