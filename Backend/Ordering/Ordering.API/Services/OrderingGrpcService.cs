using Grpc.Core;
using Ordering.Infrastructure;
using OrderService = Marketplace.Ordering.Ordering.Api.OrderService;

namespace Ordering.API.Services;

public class OrderingGrpcService : OrderService.OrderServiceBase
{
    private readonly ILogger<OrderingGrpcService> _logger;
    private readonly CatalogGrpcService _catalogService;
    private readonly OrderingContext _orderingContext;

    public OrderingGrpcService(ILogger<OrderingGrpcService> logger, CatalogGrpcService catalogService,
        OrderingContext orderingContext)
    {
        _logger = logger;
        _catalogService = catalogService;
        _orderingContext = orderingContext;
    }

    public override async Task<Marketplace.Ordering.Ordering.Api.OrderCreated> DraftOrder(Marketplace.Ordering.Ordering.Api.OrderDraft request, ServerCallContext context)
    {
        // 1. Check Buyer with given Id is exist;
        var buyer = await _catalogService.BuyerById(request.BuyerId);

        // 2. Check ProductId and ProductCount valid;
        foreach (var item in request.Items)
        {
            var product = await _catalogService.ProductById(item.ProductId);
            if (product.Count < item.Unit)
                throw new Exception("Not enough product available yet");
        }

        // 3. Generate OrderId and set status to order;
        Guid.TryParse(buyer.BuyerId, out Guid buyerId);
        Guid.TryParse(request.CardTypeId, out Guid cardTypeId);
        // var order = new Order(buyerId, buyer.UserName, buyer.BillingAddress.ToAddress(), cardTypeId,);

        // 4. Save Order To Database;
        // 5. Send Notification to User;
        // 6. return new OrderCreated object to client;
        // return base.DraftOrder(request, context);
        // return new OrderCreated();
        // return base.DraftOrder(request, context);
        throw new NotImplementedException();
    }
    
}