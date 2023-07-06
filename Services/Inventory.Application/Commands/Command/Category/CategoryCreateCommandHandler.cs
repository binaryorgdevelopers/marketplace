using Inventory.Domain.Abstractions.Repositories;
using Marketplace.Application.Common.Messages.Commands;
using Marketplace.Application.Common.Messages.Messages;
using Shared.Abstraction.Messaging;
using Shared.Models;
using Shared.Models.Constants;

namespace Marketplace.Application.Commands.Command.Category;

public class CategoryCreateCommandHandler : ICommandHandler<CategoryCreateCommand>
{
    private readonly IGenericRepository<Inventory.Domain.Entities.Category> _categoryRepository;

    public CategoryCreateCommandHandler(IGenericRepository<Inventory.Domain.Entities.Category> categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Result> Handle(CategoryCreateCommand request, CancellationToken cancellationToken)
    {
        var foundC = await _categoryRepository.GetAsync(c => c.Title == request.Title, cancellationToken);
        if (foundC is not null)
            return Result.Failure(new Error(Codes.CategoryAlreadyExists,
                $"Category with Title:'{request.Title} is already exist'"));

        var parent = await _categoryRepository.GetAsync(c => c.Id == request.ParentId, cancellationToken);

        var category = new Inventory.Domain.Entities.Category(request.Title, parent);

        await _categoryRepository.AddAsync(category, cancellationToken);

        return Result.Success(CategoryDto.FromEntity(category));
    }
}