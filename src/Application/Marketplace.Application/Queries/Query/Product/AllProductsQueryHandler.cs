using Marketplace.Application.Abstractions.Messaging;
using Marketplace.Application.Common.Messages.Messages;
using Marketplace.Domain.Abstractions.Repositories;
using Marketplace.Domain.Shared;

namespace Marketplace.Application.Queries.Query.Product;

public class ProductReadQueryHandler : ICommandHandler<ProductReadQuery>
{
    private readonly IGenericRepository<Domain.Entities.Product> _productRepository;


    public ProductReadQueryHandler(IGenericRepository<Domain.Entities.Product> productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result> Handle(ProductReadQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetAsync(c => c.Id == request.Id);
        IEnumerable<BadgeRead> badges = product.Badges.Select(c =>
            new BadgeRead(c.Id, c.Text, c.TextColor, c.BackgroundColor, c.Description, c.Type));

        CategoryRead categories = new CategoryRead(product.Category.Id, product.Category.Title,
            product.Category.ProductAmount, ArraySegment<ProductRead>.Empty);
        SellerRead sellerRead = new SellerRead(product.Seller.Id, product.Seller.Title, product.Seller.Description,
            product.Seller.Info,
            product.Seller.Username, product.Seller.FirstName, product.Seller.LastName, product.Seller.Banner,
            product.Seller.Avatar, product.Seller.Link);
        IEnumerable<BlobRead> photos = product.Photos.Select(c => new BlobRead(c.Id, c.Title, c.Extras));
        IEnumerable<CharacteristicsRead> characteristics = product.Characteristics.Select(c =>
            new CharacteristicsRead(c.Id, c.Title, c.Values.Select(x => new ColorRead(x.Id, x.Title, x.Value))));

        ProductRead productRead = new ProductRead(product.Id, product.Attributes, badges, product.Synonyms,
            product.Title, product.Description, categories, sellerRead, photos, characteristics);

        return Result.Success(productRead);
    }
}

public record ProductReadQuery(Guid Id) : ICommand;