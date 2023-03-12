using System.Linq.Expressions;
using Marketplace.Application.Abstractions.Messaging;
using Marketplace.Application.Common.Messages.Commands;
using Marketplace.Application.Common.Messages.Messages;
using Marketplace.Domain.Abstractions.Repositories;
using Marketplace.Domain.Entities;
using Marketplace.Domain.Models.Constants;
using Marketplace.Domain.Shared;

namespace Marketplace.Application.Queries.Query.Categories;

public class CategoryByTitleHandler : ICommandHandler<CategoryFilterQuery, CategoryDto>
{
    private readonly IGenericRepository<Domain.Entities.Category> _categoryRepository;

    public CategoryByTitleHandler(IGenericRepository<Domain.Entities.Category> categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<CategoryDto>> Handle(CategoryFilterQuery request, CancellationToken cancellationToken)
    {
        var includes = new Expression<Func<Category, Category>>[]
        {
            c => c.Parent
        };
        var category = _categoryRepository
            .GetWithSelect<Category, CategoryDto>(includes,
                c => c.Title == request.value,
                c => CategoryDto.FromEntity(c));
        
        return await Task.Run(() => category is not null
            ? Result.Success(category)
            : Result.Failure<CategoryDto>(new Error(Codes.InvalidCredential, "Not Found")), cancellationToken);
    }
}