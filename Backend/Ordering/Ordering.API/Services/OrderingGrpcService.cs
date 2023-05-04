using EventBus.Extensions;
using Grpc.Core;
using Marketplace.Ordering.Ordering.Api;
using MassTransit.Mediator;
using Ordering.Application;
using Ordering.Application.Commands;
using BasketItem = Ordering.Application.Models.BasketItem;

namespace Ordering.API.Services;

public class OrderingGrpcService : OrderingCheckoutService.OrderingCheckoutServiceBase
{
    // private readonly IMediator _mediator;
    // private readonly ILogger<OrderingGrpcService> _logger;
    // private readonly StateService _stateService;
    //
    // public OrderingGrpcService(IMediator mediator, ILogger<OrderingGrpcService> logger, StateService stateService)
    // {
    //     _mediator = mediator;
    //     _logger = logger;
    //     _stateService = stateService;
    // }
    //
    // public override async Task<Result> DraftOrder(OrderDraft request, ServerCallContext context)
    // {
    //     var userId = _stateService.CurrentUserId;
    //     if (Guid.TryParse(request.BuyerId, out Guid buyerId) && buyerId != userId)
    //         throw new Exception("Current user must be equal to buyer");
    //     _logger.LogInformation(
    //         "----- Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
    //         request.GetGenericTypeName(),
    //         nameof(request.BuyerId),
    //         request.BuyerId,
    //         request);
    //     CreateOrderDraftCommand orderDraftCommand =
    //         new CreateOrderDraftCommand(buyerId, new List<BasketItem>());
    //     var orderDraftDto = await _mediator.Send();
    //
    //     return base.DraftOrder(request, context);
    // }
}