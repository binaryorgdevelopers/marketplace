using Marketplace.Application.Commands.ICommand.CategoryCommand;
using Marketplace.Application.Common;
using Marketplace.Application.Common.Messages.Commands;
using Marketplace.Application.Common.Messages.Messages;
using Marketplace.Domain.Abstractions.Repositories;
using Marketplace.Domain.Entities;

namespace Marketplace.Application.Commands.Command.CategoryCommands;

public class CategoryCreateCommand : ICategoryCreateCommand
{
    private readonly IGenericRepository<Category> _categoryRepository;

    public CategoryCreateCommand(IGenericRepository<Category> categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Either<CategoryRead, Exception>> CreateCategory(CategoryCreate categoryCreate)
    {
        var category = new Category(categoryCreate.Title);
        await _categoryRepository.AddAsync(category);
        return new Either<CategoryRead, Exception>(
            new CategoryRead(category.Id, category.Title, category.ProductAmount, Array.Empty<ProductRead>()));
    }
}