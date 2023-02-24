using Marketplace.Application.Commands.ICommand.CategoryCommand;
using Marketplace.Application.Common.Messages.Commands;
using Marketplace.Application.Queries.IQuery;
using Marketplace.Application.Queries.IQuery.CategoryQueries;
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

    [HttpGet("all")]
    public ActionResult AllCategories() =>
         _categoryReadQuery.AllCategories().Match<ActionResult>(Ok, BadRequest);

    [HttpPost("create")]
    public async Task<ActionResult> CreateCategory(CategoryCreate categoryCreate) =>
        (await _categoryCreateCommand.CreateCategory(categoryCreate)).Match<ActionResult>(Ok, BadRequest);
}