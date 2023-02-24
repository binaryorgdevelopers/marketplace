using Marketplace.Application.Common;
using Marketplace.Application.Common.Messages.Messages;
using Marketplace.Application.Queries.IQuery.CategoryQueries;
using Marketplace.Domain.Abstractions.Repositories;
using Marketplace.Domain.Entities;

namespace Marketplace.Application.Queries.Query.CategoryQueries;

public class CategoryReadQuery : ICategoryReadQuery
{
    private readonly IGenericRepository<Category> _categoryRepository;

    public CategoryReadQuery(IGenericRepository<Category> categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public Either<IEnumerable<CategoryRead>, Exception> AllCategories()
    {
        var categories = _categoryRepository
            .GetWithInclude(
                c => c.Products,
                c => new CategoryRead(c.Id, c.Title, c.ProductAmount, c.Products.Select(x => new ProductRead(x.Id,
                    x.Attributes,
                    x.Badges.Select(b =>
                        new BadgeRead(b.Id, b.Text, b.TextColor, b.BackgroundColor, b.Description, b.Type)),
                    x.Synonyms, x.Title, x.Description,
                    new CategoryRead(x.Category.Id, x.Category.Title, x.Category.ProductAmount, null),
                    new SellerRead(x.Seller.Id, x.Seller.Title, x.Seller.Description, x.Seller.Info, x.Seller.Username,
                        x.Seller.FirstName, x.Seller.LastName, x.Seller.Banner, x.Seller.Avatar, x.Seller.Link),
                    x.Photos.Select(a => new BlobRead(a.Id, a.Title, a.Extras)),
                    x.Characteristics.Select(s =>
                        new CharacteristicsRead(s.Id, s.Title,
                            s.Values.Select(v => new ColorRead(v.Id, v.Title, v.Value))))))));

        return new Either<IEnumerable<CategoryRead>, Exception>(categories);
    }
}