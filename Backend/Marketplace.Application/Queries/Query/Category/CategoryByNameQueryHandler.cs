using Marketplace.Application.Abstractions.Messaging;
using Marketplace.Application.Common.Messages.Commands;
using Marketplace.Application.Common.Messages.Messages;
using Marketplace.Domain.Abstractions.Repositories;
using Marketplace.Domain.Models.Constants;
using Marketplace.Domain.Shared;

namespace Marketplace.Application.Queries.Query.Category;

public class CategoryByNameQueryHandler : ICommandHandler<CategoryByNameQuery, CategoryRead>
{
    private readonly IGenericRepository<Domain.Entities.Category> _categoryRepository;

    public CategoryByNameQueryHandler(IGenericRepository<Domain.Entities.Category> categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<CategoryRead>> Handle(CategoryByNameQuery request, CancellationToken cancellationToken)
    {
        var category = _categoryRepository.GetSingleWithInclude(c => c.Parent,
            c => new CategoryRead(c.Id, c.Title, c.ProductAmount, ArraySegment<ProductRead>.Empty),
            c => c.Title.Equals(request.Title, StringComparison.InvariantCultureIgnoreCase));
        return category is not null
            ? Result.Success(category)
            : Result.Failure<CategoryRead>(new Error("404", Codes.EntityNotExists));
    }
}