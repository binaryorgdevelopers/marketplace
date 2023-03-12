using Marketplace.Application.Abstractions.Messaging;
using Marketplace.Application.Common.Messages.Commands;
using Marketplace.Application.Common.Messages.Messages;
using Marketplace.Domain.Abstractions.Repositories;
using Marketplace.Domain.Models.Constants;
using Marketplace.Domain.Shared;

namespace Marketplace.Application.Commands.Command.Category;

public class CategoryCreateCommandHandler : ICommandHandler<CategoryCreateCommand>
{
    private readonly IGenericRepository<Domain.Entities.Category> _categoryRepository;

    public CategoryCreateCommandHandler(IGenericRepository<Domain.Entities.Category> categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Result> Handle(CategoryCreateCommand request, CancellationToken cancellationToken)
    {
        var foundC = await _categoryRepository.GetAsync(c => c.Title == request.Title);
        if (foundC is not null)
        {
            return Result.Failure(new Error(Codes.CategoryAlreadyExists,
                $"Category with Title:'{request.Title} is already exist'"));
        }

        var parent = await _categoryRepository.GetAsync(c => c.Id == request.ParentId);
        Console.WriteLine(parent);

        var category = parent is null
            ? new Domain.Entities.Category(request.Title)
            : new Domain.Entities.Category(request.Title, parent);

        await _categoryRepository.AddAsync(category);

        return Result.Success(CategoryDto.FromEntity(category));
    }
}