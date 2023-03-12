using Marketplace.Application.Abstractions.Messaging;
using Marketplace.Application.Common.Messages.Commands;
using Marketplace.Application.Common.Messages.Messages;
using Marketplace.Domain.Abstractions.Repositories;
using Marketplace.Domain.Abstractions.Services;
using Marketplace.Domain.Entities;
using Marketplace.Domain.Shared;

namespace Marketplace.Application.Commands.Command.Product;

public class ProductCreateCommandHandler : ICommandHandler<ProductCreateCommand>
{
    private readonly IGenericRepository<Domain.Entities.Product> _productRepository;

    // private readonly ICloudUploaderService _uploaderService;
    private readonly IGenericRepository<Domain.Entities.Category> _categoryRepository;
    private readonly IGenericRepository<Seller> _sellerRepository;

    public ProductCreateCommandHandler(IGenericRepository<Domain.Entities.Product> productRepository,
        ICloudUploaderService uploaderService, IGenericRepository<Domain.Entities.Category> categoryRepository,
        IGenericRepository<Seller> sellerRepository)
    {
        _productRepository = productRepository;
        // _uploaderService = uploaderService;
        _categoryRepository = categoryRepository;
        _sellerRepository = sellerRepository;
    }

    #region ProductCreate Handler

    public async Task<Result> Handle(ProductCreateCommand request, CancellationToken cancellationToken)
    {
        List<Blob> blob = new();
        foreach (var b in request.Photos)
        {
            blob.Add(new Blob("await _uploaderService.Upload(b.file, null)", b.Extras));
        }


        var category = await _categoryRepository.GetAsync(c => c.Id == request.CategoryId);
        var seller = await _sellerRepository.GetAsync(c => c.Id == request.SellerId);

        var product = new Domain.Entities.Product(
            request.Badges.Select(c => new Badge(c.Text, c.TextColor, c.BackgroundColor, c.Description)),
            request.Title,
            request.Description,
            request.CategoryId,
            request.SellerId,
            blob,
            request.Characteristics.Select(c => c.MapToChars()), seller!, category!);
        await _productRepository.AddAsync(product);

        IEnumerable<BadgeDto> badges = product.Badges.Select(c =>
            new BadgeDto(c.Id, c.Text, c.TextColor, c.BackgroundColor, c.Description, c.Type));
        CategoryDto categoryDto = new CategoryDto(product.Category.Id, product.Category.Title,
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

    #endregion
}