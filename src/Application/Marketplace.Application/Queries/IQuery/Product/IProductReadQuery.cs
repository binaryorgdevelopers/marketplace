using Marketplace.Application.Common.Messages.Messages;

namespace Marketplace.Application.Queries.IQuery.Product;

public interface IProductReadQuery
{
    Task<ProductRead> ProductById(Guid Id);
}