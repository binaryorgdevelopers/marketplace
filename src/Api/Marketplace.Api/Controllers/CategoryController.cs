using Marketplace.Api.Attributes;
using Marketplace.Application.Commands.ICommand.CategoryCommand;
using Marketplace.Application.Common.Messages.Commands;
using Marketplace.Application.Queries.IQuery.CategoryQueries;
using Marketplace.Domain.Models.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Api.Controllers;

[ApiController]
[Route("api/v1/category")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryCreateCommand _categoryCreateCommand;
    private readonly ICategoryReadQuery _categoryReadQuery;

    public CategoryController(ICategoryCreateCommand categoryCreateCommand, ICategoryReadQuery categoryReadQuery)
    {
        _categoryCreateCommand = categoryCreateCommand;
        _categoryReadQuery = categoryReadQuery;
    }

    [AllowAnonymous]
    [HttpGet("all")]
    public ActionResult AllCategories() =>
        Ok(_categoryReadQuery.AllCategories());

    [AllowAnonymous]
    [HttpGet("category/all")]
    public ActionResult CategoryWithoutProducts() =>
        Ok(_categoryReadQuery.CategoryWithoutProduct());

    [AllowAnonymous]
    [HttpGet("category/{id}")]
    public ActionResult CategoryById(Guid id)
    {
        var result = _categoryReadQuery.CategoryById(id);
        return Ok(result);
    }

    [AddRoles(Roles.Admin)]
    [HttpPost("create")]
    public async Task<ActionResult> CreateCategory(CategoryCreate categoryCreate) =>
        (await _categoryCreateCommand.CreateCategory(categoryCreate)).Match<ActionResult>(Ok, BadRequest);
}