using Authentication.Attributes;
using Authentication.Enum;
using Marketplace.Application.Common.Messages.Commands;
using Marketplace.Application.Queries.Query.Product;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers;

[ApiController]
[Route("product")]
public class ProductController : ControllerBase
{
    private readonly ISender _sender;

    public ProductController(ISender sender)
    {
        _sender = sender;
    }

    [AddRoles(Roles.Admin, Roles.Seller)]
    [HttpPost("create")]
    public async Task<IActionResult> CreateProduct(ProductCreateCommand productCreate)
    {
        var result = await _sender.Send(productCreate);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> ProductById(Guid id)
    {
        ProductByIdQuery query = new ProductByIdQuery(id);
        var result = await _sender.Send(query);
        return Ok(result);
    }
}