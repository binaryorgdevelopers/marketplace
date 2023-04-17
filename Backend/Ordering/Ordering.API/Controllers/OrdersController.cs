using System.Net;
using EventBus.Extensions;
using EventBus.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Commands;
using Ordering.Application.Queries;
using Ordering.Infrastructure.EventBus.Producers;

namespace Ordering.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IOrderQueries _orderQueries;
    private readonly IProducer<UserToken, UserId> _producer;
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(IMediator mediator,
        IOrderQueries orderQueries,
        IProducer<UserToken, UserId> producer,
        ILogger<OrdersController> logger)
    {
        _mediator = mediator;
        _orderQueries = orderQueries;
        _producer = producer;
        _logger = logger;
    }

    [HttpPut("cancel")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> CancelOrderAsync([FromBody] CancelOrderCommand command,
        [FromHeader(Name = "x-requestid")] string requestId)
    {
        if (Guid.TryParse(requestId, out Guid guid) && guid != Guid.Empty)
        {
            var requestCancelOrder = new IdentifiedCommand<CancelOrderCommand, bool>(guid, command);

            _logger.LogInformation(
                "----- Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                requestCancelOrder.GetGenericTypeName(),
                nameof(requestCancelOrder.Command.OrderNumber),
                requestCancelOrder.Command.OrderNumber,
                requestCancelOrder);

            await _mediator.Send(requestCancelOrder);
        }

        return Ok();
    }

    [HttpPut("ship")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> ShipOrderAsync([FromBody] ShipOrderCommand command,
        [FromHeader(Name = "x-requestid")] string requestId)
    {
        bool commandResult = false;

        if (Guid.TryParse(requestId, out Guid guid) && guid != Guid.Empty)
        {
            var requestShipOrder = new IdentifiedCommand<ShipOrderCommand, bool>(guid, command);

            _logger.LogInformation(
                "----- Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                requestShipOrder.GetGenericTypeName(),
                nameof(requestShipOrder.Command.OrderNumber),
                requestShipOrder.Command.OrderNumber,
                requestShipOrder);

            await _mediator.Send(requestShipOrder);
        }

        if (!commandResult)
        {
            return BadRequest();
        }

        return Ok();
    }

    [Route("{orderId:int}")]
    [HttpGet]
    [ProducesResponseType(typeof(Order), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult> GetOrderAsync(int orderId)
    {
        try
        {
            //Todo: It's good idea to take advantage of GetOrderByIdQuery and handle by GetCustomerByIdQueryHandler
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
    [ProducesResponseType(typeof(IEnumerable<OrderSummary>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<OrderSummary>>> GetOrdersAsync(
        [FromHeader(Name = "Authorizarion")] string token)
    {
        var userid = (await _producer.Handle(new UserToken(token))).userId;
        var orders = await _orderQueries.GetOrderFromUserAsync(userid);

        return Ok(orders);
    }

    [Route("cardtypes")]
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CardType>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<CardType>>> GetCardTypesAsync()
    {
        var cardTypes = await _orderQueries.GetCardTypeAsync();

        return Ok(cardTypes);
    }

    [Route("draft")]
    [HttpPost]
    public async Task<ActionResult<OrderDraftDto>> CreateOrderDraftFromBasketDataAsync(
        [FromBody] CreateOrderDraftCommand createOrderDraftCommand)
    {
        _logger.LogInformation(
            "----- Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
            createOrderDraftCommand.GetGenericTypeName(),
            nameof(createOrderDraftCommand.BuyerId),
            createOrderDraftCommand.BuyerId,
            createOrderDraftCommand);
        await _mediator.Send(createOrderDraftCommand);

        return Ok();
    }
}