using Authentication.Attributes;
using Authentication.Enum;
using Inventory.Api.Extensions;
using Inventory.Domain.Shared;
using Marketplace.Application.Common.Messages.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers;
[ApiController]
[Route("api/card")]
public class CardController : ControllerBase
{
    private readonly ISender _sender;

    public CardController(ISender sender)
    {
        _sender = sender;
    }

    [AddRoles(Roles.Admin, Roles.Customer, Roles.User)]
    [HttpPost("bind")]
    public async ValueTask<IActionResult> BindCardToUser(BindCardToUserCommand cardToUserCommand)
        =>
            await Result
                .Create(cardToUserCommand)
                .Bind(c => _sender.Send(c))
                .Match(Ok, BadRequest);

    [AddRoles(Roles.Admin, Roles.Customer, Roles.User)]
    [HttpGet("{guid}")]
    public async ValueTask<IActionResult> GetUserCards(Guid guid)
        =>
            await Result
                .Create(new CardByUserIdCommand(guid))
                .Bind(c => _sender.Send(c))
                .Match(Ok, BadRequest);
}