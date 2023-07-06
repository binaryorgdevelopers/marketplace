using Inventory.Domain.Abstractions.Repositories;
using Inventory.Domain.Entities;
using Marketplace.Application.Common.Messages.Messages;
using Shared.Abstraction.Messaging;
using Shared.Models;
using Shared.Models.Constants;

namespace Marketplace.Application.Queries.Query.Categories;

public class CategoryByIdQueryHandler : ICommandHandler<CategoryByIdQuery>
{
    private readonly IGenericRepository<Category> _categoryRepository;

    public CategoryByIdQueryHandler(IGenericRepository<Category> categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }


    public async Task<Result> Handle(CategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = _categoryRepository
            .GetWithSelect<CategoryDto>(
                c => c.Id == request.Id,
                c => CategoryDto.FromEntity(c),
                i => i.Parent);

        return await Task.Run(() => category is not null
            ? Result.Success(category)
            : Result.Failure<CategoryDto>(new Error(Codes.InvalidCredential,
                $"Category with id:'{request.Id}' not found")), cancellationToken);
    }
}

public record CategoryByIdQuery(Guid Id) : ICommand;