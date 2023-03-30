using Marketplace.Api.Attributes;
using Marketplace.Api.Extensions;
using Marketplace.Application.Common.Builder.Models;
using Marketplace.Application.Common.Messages.Commands;
using Marketplace.Application.Common.Messages.Messages;
using Marketplace.Application.Queries.Query.Product;
using Marketplace.Domain.Entities;
using Marketplace.Domain.Models.Constants;
using Marketplace.Domain.Shared;
using Marketplace.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Api.Controllers;

[ApiController]
[Route("api/v1/product")]
public class ProductController : ControllerBase
{
    private readonly ISender _sender;
    private readonly SearchService<Product> _searchService;

    public ProductController(ISender sender, SearchService<Product> searchService)
    {
        _sender = sender;
        _searchService = searchService;
    }

    [AddRoles(Roles.Admin, Roles.Seller)]
    [HttpPost("create")]
    public async Task<IActionResult> CreateProduct(ProductCreate productCreate) =>
        await Result.Create(new ProductCreateCommand(
            productCreate.SellerId, productCreate.CategoryId,
            productCreate.Title, productCreate.Description,
            productCreate.Characteristics,
            productCreate.Badges, productCreate.Photos
        )).Bind(command => _sender.Send(command)).Match(Ok, BadRequest);

    [HttpGet("{id}")]
    public async Task<ActionResult> ProductById(Guid id)
    {
        ProductByIdQuery query = new ProductByIdQuery(id);
        var result = await _sender.Send(query);
        return Ok(result);
    }

    [HttpPost("filter")]
    public async Task<IActionResult> GetByFilter([FromBody] EntityQueryOptions<Product> filter)
    {
        var data = await _searchService.GetByFilterAsync(filter);
        var enumerable = data as Product[] ?? data.ToArray();
        return enumerable.Any() ? Ok(enumerable.Select(ProductDto.FromEntity)) : NotFound();
    }
}

public class ProductCreate
{
    public Guid SellerId { get; set; }
    public Guid CategoryId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public IEnumerable<CharacteristicsCreate> Characteristics { get; set; }
    public IEnumerable<BadgeCreate> Badges { get; set; }
    public IEnumerable<BlobCreate> Photos { get; set; }
}