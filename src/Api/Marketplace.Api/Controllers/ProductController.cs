using Marketplace.Application.Commands.ICommand.Product;
using Marketplace.Application.Common.Messages.Commands;
using Marketplace.Application.Queries.IQuery.Product;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Api.Controllers;

[ApiController]
[Route("api/v1/product")]
public class ProductController : ControllerBase
{
    private readonly IProductCreateCommand _productCreateCommand;
    private readonly IProductReadQuery _productReadQuery;

    public ProductController(IProductCreateCommand productCreateCommand, IProductReadQuery productReadQuery)
    {
        _productCreateCommand = productCreateCommand;
        _productReadQuery = productReadQuery;
    }

    [HttpPost("create")]
    public async Task<ActionResult> CreateProduct([FromForm] ProductCreate productCreate)
    {
        var request = await _productCreateCommand.ProductCreate(productCreate);
        return request.Match<ActionResult>(Ok, BadRequest);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> ProductById(Guid id) => Ok(_productReadQuery.ProductById(id));
}