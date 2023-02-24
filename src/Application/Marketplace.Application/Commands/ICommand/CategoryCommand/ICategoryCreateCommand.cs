using Marketplace.Application.Common;
using Marketplace.Application.Common.Messages.Commands;
using Marketplace.Application.Common.Messages.Messages;

namespace Marketplace.Application.Commands.ICommand.CategoryCommand;

public interface ICategoryCreateCommand
{
    Task<Either<CategoryRead, Exception>> CreateCategory(CategoryCreate categoryCreate);
}