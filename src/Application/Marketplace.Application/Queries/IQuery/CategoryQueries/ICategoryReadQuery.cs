using Marketplace.Application.Common;
using Marketplace.Application.Common.Messages.Messages;

namespace Marketplace.Application.Queries.IQuery.CategoryQueries;

public interface ICategoryReadQuery
{
    IEnumerable<CategoryRead> AllCategories();
    IEnumerable<CategoryRead>CategoryWithoutProduct();
    CategoryRead? CategoryById(Guid Id);
}