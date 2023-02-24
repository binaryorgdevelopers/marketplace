using Marketplace.Application.Commands.ICommand.Product;
using Marketplace.Application.Common.Messages.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Api.Controllers;

[ApiController]
[Route("api/v1/product")]
public class ProductController : ControllerBase
{
    private readonly IProductCreateCommand _productCreateCommand;

    public ProductController(IProductCreateCommand productCreateCommand)
    {
        _productCreateCommand = productCreateCommand;
    }

    [HttpPost("create")]
    public async Task<ActionResult> CreateProduct([FromForm]ProductCreate productCreate) =>
        (await _productCreateCommand.ProductCreate(productCreate)).Match<ActionResult>(Ok, BadRequest);
}