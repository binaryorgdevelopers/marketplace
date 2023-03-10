using Marketplace.Application.Abstractions.Messaging;
using Marketplace.Application.Common.Messages.Messages;
using Marketplace.Domain.Abstractions.Repositories;
using Marketplace.Domain.Shared;

namespace Marketplace.Application.Queries.Query.Category;

public class CategoryByIdQueryHandler : ICommandHandler<CategoryByIdQuery>
{
    private readonly IGenericRepository<Domain.Entities.Category> _categoryRepository;

    public CategoryByIdQueryHandler(IGenericRepository<Domain.Entities.Category> categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Result> Handle(CategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = _categoryRepository
            .GetSingleWithInclude(
                c => c.Products,
                c => new CategoryRead(c.Id, c.Title, c.ProductAmount, c.Products.Select(x => new ProductRead(x.Id,
                    x.Attributes,
                    x.Badges.Select(b =>
                        new BadgeRead(b.Id, b.Text, b.TextColor, b.BackgroundColor, b.Description, b.Type)),
                    x.Synonyms, x.Title, x.Description,
                    new CategoryRead(x.Category.Id, x.Category.Title, x.Category.ProductAmount, null),
                    new SellerRead(x.Seller.Id, x.Seller.Title, x.Seller.Description, x.Seller.Info,
                        x.Seller.Username,
                        x.Seller.FirstName, x.Seller.LastName, x.Seller.Banner, x.Seller.Avatar, x.Seller.Link),
                    x.Photos.Select(a => new BlobRead(a.Id, a.Title, a.Extras)),
                    x.Characteristics.Select(s =>
                        new CharacteristicsRead(s.Id, s.Title,
                            s.Values.Select(v => new ColorRead(v.Id, v.Title, v.Value))))))),
                p => p.Id == request.Id);
        return Result.Success(category);
    }
}

public record CategoryByIdQuery(Guid Id) : ICommand;