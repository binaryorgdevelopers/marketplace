using Marketplace.Api.Attributes;
using Marketplace.Application.Common.Messages.Commands;
using Marketplace.Application.Queries.Query.Category;
using Marketplace.Domain.Models.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Api.Controllers;

[ApiController]
[Route("api/v1/category")]
public class CategoryController : ControllerBase
{
    private readonly ISender _sender;

    public CategoryController(ISender sender) => _sender = sender;


    // [AllowAnonymous]
    // [HttpGet("all")]
    // public ActionResult AllCategories() =>
    //     Ok(_categoryReadQuery.AllCategories());

    // [AllowAnonymous]
    // [HttpGet("category/all")]
    // public ActionResult CategoryWithoutProducts() =>
    //     Ok(_categoryReadQuery.CategoryWithoutProduct());

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
}