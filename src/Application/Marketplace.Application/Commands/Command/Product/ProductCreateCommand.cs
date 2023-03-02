using Marketplace.Application.Commands.ICommand.Product;
using Marketplace.Application.Common;
using Marketplace.Application.Common.Messages.Commands;
using Marketplace.Application.Common.Messages.Messages;
using Marketplace.Domain.Abstractions.Repositories;
using Marketplace.Domain.Abstractions.Services;
using Marketplace.Domain.Entities;

namespace Marketplace.Application.Commands.Command.Product;

public class ProductCreateCommand : IProductCreateCommand
{
    private readonly IGenericRepository<Domain.Entities.Product> _productRepository;
    private readonly ICloudUploaderService _uploaderService;
    private readonly IGenericRepository<Category> _categoryRepository;
    private readonly IGenericRepository<Seller> _sellerRepository;

    public ProductCreateCommand(IGenericRepository<Domain.Entities.Product> productRepository,
        ICloudUploaderService uploaderService, IGenericRepository<Category> categoryRepository,
        IGenericRepository<Seller> sellerRepository)
    {
        this._productRepository = productRepository;
        _uploaderService = uploaderService;
        _categoryRepository = categoryRepository;
        _sellerRepository = sellerRepository;
    }

    public async Task<Either<ProductRead, Exception>> ProductCreate(ProductCreate productCreate)
    {
        List<Blob> blob = new();
        foreach (var b in productCreate.Photos)
        {
            blob.Add(new Blob(await _uploaderService.Upload(b.file, null), b.Extras));
        }

        var category = await _categoryRepository.GetAsync(c => c.Id == productCreate.CategoryId);
        var seller = await _sellerRepository.GetAsync(c => c.Id == productCreate.SellerId);

        var product = new Domain.Entities.Product(
            productCreate.Badges.Select(c => new Badge(c.Text, c.TextColor, c.BackgroundColor, c.Description)),
            productCreate.Title,
            productCreate.Description,
            productCreate.CategoryId,
            productCreate.SellerId,
            blob,
            productCreate.Characteristics.Select(c => c.MapToChars()), seller!, category!);
        await _productRepository.AddAsync(product);

        IEnumerable<BadgeRead> badges = product.Badges.Select(c =>
            new BadgeRead(c.Id, c.Text, c.TextColor, c.BackgroundColor, c.Description, c.Type));
        CategoryRead categoryRead = new CategoryRead(product.Category.Id, product.Category.Title,
            product.Category.ProductAmount, ArraySegment<ProductRead>.Empty);
        SellerRead sellerRead = new SellerRead(product.Seller.Id, product.Seller.Title, product.Seller.Description,
            product.Seller.Info,
            product.Seller.Username, product.Seller.FirstName, product.Seller.LastName, product.Seller.Banner,
            product.Seller.Avatar, product.Seller.Link);
        IEnumerable<BlobRead> photos = product.Photos.Select(c => new BlobRead(c.Id, c.Title, c.Extras));
        IEnumerable<CharacteristicsRead> characteristics = product.Characteristics.Select(c =>
            new CharacteristicsRead(c.Id, c.Title, c.Values.Select(x => new ColorRead(x.Id, x.Title, x.Value))));


        return new Either<ProductRead, Exception>(new ProductRead(product.Id, product.Attributes,
            badges, product.Synonyms, product.Title, product.Description, categoryRead, sellerRead, photos,
            characteristics));
    }
}