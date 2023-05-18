using Ordering.API.Extensions;
using Ordering.Inventory.Api;

namespace Ordering.API.Services;

public class CatalogGrpcService
{
    private readonly CatalogService.CatalogServiceClient _catalogService;
    private readonly ILogger<CatalogGrpcService> _logger;

    public CatalogGrpcService(CatalogService.CatalogServiceClient catalogService, ILogger<CatalogGrpcService> logger)
    {
        _catalogService = catalogService;
        _logger = logger;
    }

    public async ValueTask<Buyer> BuyerById(string id)
    {
        var result = await _catalogService.BuyerByIdAsync(id.ToBuyerByIdRequest());
        return result;
    }

    public async ValueTask<Product> ProductById(string id)
    {
        var result = await _catalogService.ProductByIdAsync(id.ToProductByIdRequest());
        return result;
    }
}