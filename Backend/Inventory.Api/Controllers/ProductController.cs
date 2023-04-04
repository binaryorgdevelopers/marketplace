﻿using Inventory.Api.Attributes;
using Inventory.Api.Extensions;
using Marketplace.Application.Common.Builder.Models;
using Marketplace.Application.Common.Messages.Commands;
using Marketplace.Application.Common.Messages.Messages;
using Marketplace.Application.Queries.Query.Product;
using Inventory.Domain.Entities;
using Inventory.Domain.Models.Constants;
using Inventory.Domain.Shared;
using Marketplace.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers;

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
    public async Task<IActionResult> CreateProduct(ProductCreateCommand productCreate) =>
        await Result.Create(productCreate)
            .Bind(command => _sender.Send(command))
            .Match(Ok, BadRequest);

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