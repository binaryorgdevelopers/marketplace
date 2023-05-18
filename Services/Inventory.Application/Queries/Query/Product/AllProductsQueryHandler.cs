using Inventory.Domain.Abstractions.Repositories;
using Inventory.Domain.Shared;
using Marketplace.Application.Abstractions.Messaging;
using Marketplace.Application.Common.Messages.Messages;

namespace Marketplace.Application.Queries.Query.Product;

public class ProductReadQueryHandler : ICommandHandler<ProductReadQuery>
{
    private readonly IGenericRepository<Inventory.Domain.Entities.Product> _productRepository;


    public ProductReadQueryHandler(IGenericRepository<Inventory.Domain.Entities.Product> productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result> Handle(ProductReadQuery request, CancellationToken cancellationToken)
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
        IEnumerable<CharacteristicsDto> characteristics = product.Characteristics.Select(c =>
            new CharacteristicsDto(c.Id, c.Title, c.Values.Select(x => new ColorRead(x.Id, x.Title, x.Value))));

        ProductDto productDto = new ProductDto(product.Id, product.Attributes, badges, product.Synonyms,
            product.Title, product.Description, categories, sellerDto, photos, characteristics);

        return Result.Success(productDto);
    }
}

public record ProductReadQuery(Guid Id) : ICommand;