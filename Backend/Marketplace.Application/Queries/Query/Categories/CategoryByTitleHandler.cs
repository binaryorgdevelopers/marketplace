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
    private readonly IGenericRepository<Category> _categoryRepository;

    public CategoryByTitleHandler(IGenericRepository<Category> categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<CategoryDto>> Handle(CategoryFilterQuery request, CancellationToken cancellationToken)
    {
        var category = _categoryRepository
            .GetWithSelect<CategoryDto>(
                c => c.Title == request.value,
                c => CategoryDto.FromEntity(c),
                i => i.Parent);

        return await Task.Run(() => category is not null
            ? Result.Success(category)
            : Result.Failure<CategoryDto>(new Error(Codes.InvalidCredential,
                $"Category with Name:'{request.value}' not found")), cancellationToken);
    }
}