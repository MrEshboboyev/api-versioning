﻿using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.Mvc;
using Products.Api.Data;
using Products.Api.Features;
using Products.Api.Models;

namespace Products.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[ApiVersion("2")]
[Route("api/v{version:apiVersion}/products")]
public sealed class ProductsController(
    ProductsDbContext context,
    ILogger<ProductsController> logger,
    IFeatureManager featureManager) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<ProductResponseV1>>> GetProducts()
    {
        var response = await context.Products
            .Select(p => new ProductResponseV1
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price
            })
            .ToListAsync();

        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    [MapToApiVersion("1")]
    [FeatureGate(FeatureFlags.UseV1ProductApi)]
    public async Task<ActionResult<ProductResponseV1>> GetProductV1(Guid id)
    {
        if (!await featureManager.IsEnabledAsync(FeatureFlags.UseV1ProductApi))
        {
            return NotFound();
        }
        
        var response = await context.Products
            .Where(p => p.Id == id)
            .Select(p => new ProductResponseV1
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price
            })
            .FirstOrDefaultAsync();

        if (response is null)
        {
            return NotFound();
        }

        logger.LogInformation("Retrieved product {ProductId} using V1 endpoint", id);
        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    [MapToApiVersion("2")]
    [FeatureGate(FeatureFlags.UseV2ProductApi)]
    public async Task<ActionResult<ProductResponseV2>> GetProductV2(Guid id)
    {
        if (!await featureManager.IsEnabledAsync(FeatureFlags.UseV2ProductApi))
        {
            return NotFound();
        }
        
        var response = await context.Products
            .Where(p => p.Id == id)
            .Select(p => new ProductResponseV2
            {
                Id = p.Id,
                Product = new ProductInfoV2
                {
                    Name = p.Name,
                    DisplayName = p.DisplayName,
                    Pricing = new PricingInfoV2
                    {
                        Amount = p.Price,
                        Currency = p.Currency,
                        Discounted = p.IsDiscounted
                    }
                },
                Inventory = new InventoryInfoV2
                {
                    InStock = p.InStock,
                    Quantity = p.Quantity
                }
            })
            .FirstOrDefaultAsync();

        if (response is null)
        {
            return NotFound();
        }

        logger.LogInformation("Retrieved product {ProductId} using V2 endpoint", id);
        return Ok(response);
    }
}