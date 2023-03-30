using Marketplace.Application.Abstractions.Messaging;

namespace Marketplace.Application.Queries.Query.Product;

public class ProductSearchQueryHandler
{
}

public sealed record ProductSearchQuery(string field);