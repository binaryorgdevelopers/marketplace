using Authentication.Attributes;
using Authentication.Enum;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Commands;
using Ordering.Application.Queries;

namespace Ordering.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IOrderQueries _orderQueries;
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(IMediator mediator,
        IOrderQueries orderQueries,
        ILogger<OrdersController> logger)
    {
        _mediator = mediator;
        _orderQueries = orderQueries;
        _logger = logger;
    }

    [HttpPut("cancel")]
    [AddRoles(Roles.Admin, Roles.Seller)]
    public async Task<IActionResult> CancelOrderAsync([FromBody] CancelOrderCommand command,
        [FromHeader(Name = "requestId")] string requestId)
    {
        if (Guid.TryParse(requestId, out Guid guid) && guid != Guid.Empty)
        {

            await _mediator.Send(command);
        }

        return Ok();
    }

    [HttpPut("ship")]
    [AddRoles(Roles.Admin, Roles.Seller)]
    public async Task<IActionResult> ShipOrderAsync([FromBody] ShipOrderCommand command,
        [FromHeader(Name = "requestId")] string requestId)
    {
        bool commandResult = false;

        if (Guid.TryParse(requestId, out Guid guid) && guid != Guid.Empty)
        {
            var requestShipOrder = new IdentifiedCommand<ShipOrderCommand, bool>(guid, command);
            await _mediator.Send(requestShipOrder);
        }

        return Ok();
    }

    [Route("{orderId:int}")]
    [HttpGet]
    [AddRoles(Roles.Admin, Roles.Seller, Roles.User)]
    public async Task<ActionResult> GetOrderAsync(Guid orderId)
    {
        try
        {
            //var order customer = await _mediator.Send(new GetOrderByIdQuery(orderId));
            var order = await _orderQueries.GetOrderAsync(orderId);

            return Ok(order);
        }
        catch
        {
            return NotFound();
        }
    }

    [HttpGet]
    [AddRoles(Roles.Admin, Roles.Seller)]
    public async Task<ActionResult<IEnumerable<OrderSummary>>> GetOrdersAsync(Guid userId)
    {
        var orders = await _orderQueries.GetOrderFromUserAsync(userId);

        return Ok(orders);
    }

    [HttpGet("cardtypes")]
    [AddRoles(Roles.Admin, Roles.Seller, Roles.User)]
    public async Task<ActionResult<IEnumerable<CardType>>> GetCardTypesAsync()
    {
        var cardTypes = await _orderQueries.GetCardTypeAsync();

        return Ok(cardTypes);
    }

    [HttpPost("draft")]
    [AddRoles(Roles.Admin, Roles.Seller, Roles.User)]
    public async Task<ActionResult<OrderDraftDto>> CreateOrderDraftFromBasketDataAsync(
        CreateOrderDraftCommand createOrderDraftCommand)
    {
        var orderDraftDto = await _mediator.Send(createOrderDraftCommand);

        return Ok(orderDraftDto);
    }
}