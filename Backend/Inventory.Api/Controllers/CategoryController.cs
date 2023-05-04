using Authentication.Attributes;
using Authentication.Enum;
using Inventory.Api.Extensions;
using Marketplace.Application.Common.Messages.Commands;
using Marketplace.Application.Queries.Query.Categories;
using Inventory.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers;

[ApiController]
[Route("api/v1/category")]
public class CategoryController : ControllerBase
{
    private readonly ISender _sender;

    public CategoryController(ISender sender) => _sender = sender;

    [AllowAnonymous]
    [HttpGet("category/{id}")]
    public async Task<ActionResult> CategoryById(Guid id)
    {
        var result = await _sender.Send(new CategoryByIdQuery(id));
        return Ok(result);
    }

    [AddRoles(Roles.Admin)]
    [HttpPost("create")]
    public async Task<ActionResult> CreateCategory(CategoryCreateCommand categoryCreateCommand)
    {
        var result = await _sender.Send(categoryCreateCommand);
        return Ok(result);
    }

    [HttpPost("filter")]
    public async Task<IActionResult> CategoryByName(FilterCommand filter) =>
        await Result
            .Create(new CategoryFilterQuery(filter.Field, filter.Value))
            .Bind(command => _sender.Send(command))
            .Match(Ok, BadRequest);

    [HttpGet]
    public async Task<IActionResult> CategoryByName([FromQuery] string name)
    {
        return await Result
            .Create(new CategoryFilterQuery("", name))
            .Bind(command => _sender.Send(command))
            .Match(Ok, BadRequest);
    }
}