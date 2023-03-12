using Marketplace.Application.Abstractions.Messaging;
using Marketplace.Domain.Abstractions.Repositories;
using Marketplace.Domain.Shared;

namespace Marketplace.Application.Queries.Query.Categories;

public class CategoryByIdQueryHandler : ICommandHandler<CategoryByIdQuery>
{
    private readonly IGenericRepository<Domain.Entities.Category> _categoryRepository;

    public CategoryByIdQueryHandler(IGenericRepository<Domain.Entities.Category> categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Result> Handle(CategoryByIdQuery request, CancellationToken cancellationToken)
    {
        // var category = _categoryRepository.GetSingleWithInclude<CategoryDto>("Products", c => c.Id == request.Id);
        return Result.Success("category");
    }
}

public record CategoryByIdQuery(Guid Id) : ICommand;