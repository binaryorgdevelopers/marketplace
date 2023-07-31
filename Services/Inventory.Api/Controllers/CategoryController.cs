using Authentication.Attributes;
using Authentication.Enum;
using Marketplace.Application.Common.Messages.Commands;
using Marketplace.Application.Queries.Query.Categories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers;

[ApiController]
[Route("category")]
public class CategoryController : ControllerBase
{
    private readonly ISender _sender;

    public CategoryController(ISender sender) => _sender = sender;

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
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
}