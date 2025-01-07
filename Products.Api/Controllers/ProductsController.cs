using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Products.Api.Data;
using Products.Api.Models;

namespace Products.Api.Controllers;

[ApiController]
[Route("api/products")]
public sealed class ProductsController(
    ProductsDbContext context,
    ILogger<ProductsController> logger)
    : ControllerBase
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
    public async Task<ActionResult<ProductResponseV1>> GetProductV1(Guid id)
    {
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
}