using Marketplace.Application.Common.Messages.Commands;
using Marketplace.Application.Queries.Query.Product;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Api.Controllers;

[ApiController]
[Route("api/v1/product")]
public class ProductController : ControllerBase
{
    private readonly ISender _sender;

    public ProductController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("create")]
    public async Task<ActionResult> CreateProduct([FromForm] ProductCreateCommand productCreateCommand)
    {
        var result=await _sender.Send(productCreateCommand);
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