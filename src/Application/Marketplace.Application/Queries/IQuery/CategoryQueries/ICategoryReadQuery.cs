using Marketplace.Application.Common;
using Marketplace.Application.Common.Messages.Messages;

namespace Marketplace.Application.Queries.IQuery.CategoryQueries;

public interface ICategoryReadQuery
{
    Either<IEnumerable<CategoryRead>, Exception> AllCategories();
}