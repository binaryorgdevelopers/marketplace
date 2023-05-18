using Inventory.Domain.Abstractions.Repositories;
using Inventory.Domain.Entities;
using Inventory.Domain.Shared;
using Marketplace.Application.Abstractions.Messaging;

namespace Marketplace.Application.Queries.Query.Categories;

public partial class CategoryReadQueryHandler : ICommandHandler<CategoryReadQuery>
{
    private readonly IGenericRepository<Category> _categoryRepository;

    public CategoryReadQueryHandler(IGenericRepository<Category> categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }


    public async Task<Result> Handle(CategoryReadQuery request, CancellationToken cancellationToken)
    {
        // var categories = _categoryRepository
        //     .GetWithInclude(
        //         c => c.Products,
        //         c =>
        //             new CategoryDto(c.Id, c.Title, c.ProductAmount, c.Products.Select(x =>
        //                 new ProductDto(x.Id, x.Attributes, x.Badges.Select(b =>
        //                         new BadgeDto(b.Id, b.Text, b.TextColor, b.BackgroundColor, b.Description, b.Type)),
        //                     x.Synonyms, x.Title, x.Description,
        //                     new CategoryDto(x.Category.Id, x.Category.Title, x.Category.ProductAmount,ArraySegment<ProductDto>.Empty, Guid.Empty),
        //                     new SellerDto(x.Seller.Id, x.Seller.Title, x.Seller.Description, x.Seller.Info,
        //                         x.Seller.Username,
        //                         x.Seller.FirstName, x.Seller.LastName, x.Seller.Banner, x.Seller.Avatar,
        //                         x.Seller.Link),
        //                     x.Photos.Select(a => new BlobDto(a.Id, a.Title, a.Extras)),
        //                     x.Characteristics.Select(s =>
        //                         new CharacteristicsDto(s.Id, s.Title,
        //                             s.Values.Select(v => new ColorRead(v.Id, v.Title, v.Value))))))));

        return await Task.FromResult(Result.Success("categories"));
    }
    // public IEnumerable<CategoryRead> CategoryWithoutProduct()
    // {
    //     var categories = _categoryRepository.GetAll().Select(c =>
    //         new CategoryRead(c.Id, c.Title, c.ProductAmount, ArraySegment<ProductRead>.Empty));
    //     return (categories);
    // }
}
public record CategoryReadQuery : ICommand;