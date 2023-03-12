using Marketplace.Application.Abstractions.Messaging;
using Marketplace.Application.Common.Messages.Messages;
using Marketplace.Domain.Abstractions.Repositories;
using Marketplace.Domain.Shared;

namespace Marketplace.Application.Queries.Query.Category;

public class CategoryReadQueryHandler : ICommandHandler<CategoryReadQuery>
{
    private readonly IGenericRepository<Domain.Entities.Category> _categoryRepository;

    public CategoryReadQueryHandler(IGenericRepository<Domain.Entities.Category> categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }


    public async Task<Result> Handle(CategoryReadQuery request, CancellationToken cancellationToken)
    {
        var categories = _categoryRepository
            .GetWithInclude(
                c => c.Products,
                c =>
                    new CategoryRead(c.Id, c.Title, c.ProductAmount, c.Products.Select(x =>
                        new ProductRead(x.Id, x.Attributes, x.Badges.Select(b =>
                                new BadgeRead(b.Id, b.Text, b.TextColor, b.BackgroundColor, b.Description, b.Type)),
                            x.Synonyms, x.Title, x.Description,
                            new CategoryRead(x.Category.Id, x.Category.Title, x.Category.ProductAmount, null),
                            new SellerRead(x.Seller.Id, x.Seller.Title, x.Seller.Description, x.Seller.Info,
                                x.Seller.Username,
                                x.Seller.FirstName, x.Seller.LastName, x.Seller.Banner, x.Seller.Avatar,
                                x.Seller.Link),
                            x.Photos.Select(a => new BlobRead(a.Id, a.Title, a.Extras)),
                            x.Characteristics.Select(s =>
                                new CharacteristicsRead(s.Id, s.Title,
                                    s.Values.Select(v => new ColorRead(v.Id, v.Title, v.Value))))))));

        return await Task.FromResult(Result.Success(categories));
    }
    // public IEnumerable<CategoryRead> CategoryWithoutProduct()
    // {
    //     var categories = _categoryRepository.GetAll().Select(c =>
    //         new CategoryRead(c.Id, c.Title, c.ProductAmount, ArraySegment<ProductRead>.Empty));
    //     return (categories);
    // }
}

public record CategoryReadQuery : ICommand;