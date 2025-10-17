using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.FeatureManagement;
using Products.Api.Contracts;
using Products.Api.Data;
using Products.Api.Features;
using Products.Api.Models;

namespace Products.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
[ApiVersion("3.0")]
[Route("api/v{version:apiVersion}/products")]
public sealed class ProductsController(
    ProductsDbContext context,
    ILogger<ProductsController> logger,
    IFeatureManager featureManager) : ControllerBase
{
    // GET all products - available in all versions but returns different response types
    [HttpGet]
    public async Task<ActionResult> GetProducts(ApiVersion apiVersion)
    {
        if (apiVersion.MajorVersion == 1)
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
        else if (apiVersion.MajorVersion == 2)
        {
            var products = await context.Products.ToListAsync();
            var response = products.Select(p => new ProductResponseV2
            {
                Id = p.Id,
                Product = new ProductInfoV2
                {
                    Name = p.Name,
                    DisplayName = !string.IsNullOrEmpty(p.DisplayName) ? p.DisplayName : p.Name,
                    Pricing = new PricingInfoV2
                    {
                        Amount = p.Price,
                        Currency = !string.IsNullOrEmpty(p.Currency) ? p.Currency : "USD",
                        Discounted = p.IsDiscounted
                    }
                },
                Inventory = new InventoryInfoV2
                {
                    InStock = p.InStock,
                    Quantity = p.Quantity
                }
            }).ToList();
            return Ok(response);
        }
        else
        {
            var products = await context.Products.ToListAsync();
            var response = products.Select(p => new ProductResponseV3
            {
                Id = p.Id,
                Product = new ProductInfoV3
                {
                    Name = p.Name,
                    DisplayName = !string.IsNullOrEmpty(p.DisplayName) ? p.DisplayName : p.Name,
                    Description = !string.IsNullOrEmpty(p.Description) ? p.Description : "",
                    Tags = !string.IsNullOrEmpty(p.Tags) ? p.Tags.Split(',').ToList() : new List<string>(),
                    Pricing = new PricingInfoV3
                    {
                        Amount = p.Price,
                        Currency = !string.IsNullOrEmpty(p.Currency) ? p.Currency : "USD",
                        Discounted = p.IsDiscounted,
                        DiscountedAmount = p.DiscountedPrice,
                        PriceHistory = new List<PriceHistoryEntryV3>()
                    },
                    Variants = new List<VariantInfoV3>()
                },
                Inventory = new InventoryInfoV3
                {
                    InStock = p.InStock,
                    Quantity = p.Quantity,
                    ReservedQuantity = 0,
                    Warehouse = new WarehouseInfoV3
                    {
                        Location = "Primary Warehouse",
                        Code = "WH-001"
                    },
                    InventoryHistory = new List<InventoryHistoryEntryV3>()
                },
                Analytics = new AnalyticsInfoV3
                {
                    Views = p.Views,
                    Purchases = p.Purchases,
                    Rating = p.Rating,
                    ReviewsCount = p.ReviewsCount,
                    TopReviews = new List<ReviewInfoV3>()
                },
                Category = new CategoryInfoV3
                {
                    PrimaryCategory = !string.IsNullOrEmpty(p.Category) ? p.Category : "General",
                    SubCategories = new List<string>(),
                    Department = !string.IsNullOrEmpty(p.Department) ? p.Department : "Default"
                }
            }).ToList();
            return Ok(response);
        }
    }

    // GET a specific product by ID - version-specific implementations
    [HttpGet("{id:guid}")]
    public async Task<ActionResult> GetProduct(Guid id, ApiVersion apiVersion)
    {
        if (apiVersion.MajorVersion == 1)
        {
            if (!await featureManager.IsEnabledAsync(FeatureFlags.UseV1ProductApi))
            {
                return NotFound();
            }
            
            var product = await context.Products.FirstOrDefaultAsync(p => p.Id == id);
            
            if (product is null)
            {
                return NotFound();
            }

            var response = new ProductResponseV1
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price
            };

            logger.LogInformation("Retrieved product {ProductId} using V1 endpoint", id);
            return Ok(response);
        }
        else if (apiVersion.MajorVersion == 2)
        {
            if (!await featureManager.IsEnabledAsync(FeatureFlags.UseV2ProductApi))
            {
                return NotFound();
            }
            
            var product = await context.Products.FirstOrDefaultAsync(p => p.Id == id);
            
            if (product is null)
            {
                return NotFound();
            }

            var response = new ProductResponseV2
            {
                Id = product.Id,
                Product = new ProductInfoV2
                {
                    Name = product.Name,
                    DisplayName = !string.IsNullOrEmpty(product.DisplayName) ? product.DisplayName : product.Name,
                    Pricing = new PricingInfoV2
                    {
                        Amount = product.Price,
                        Currency = !string.IsNullOrEmpty(product.Currency) ? product.Currency : "USD",
                        Discounted = product.IsDiscounted
                    }
                },
                Inventory = new InventoryInfoV2
                {
                    InStock = product.InStock,
                    Quantity = product.Quantity
                }
            };

            logger.LogInformation("Retrieved product {ProductId} using V2 endpoint", id);
            return Ok(response);
        }
        else
        {
            // V3 is the latest version, no feature flag check needed
            var product = await context.Products.FirstOrDefaultAsync(p => p.Id == id);
            
            if (product is null)
            {
                return NotFound();
            }

            var response = new ProductResponseV3
            {
                Id = product.Id,
                Product = new ProductInfoV3
                {
                    Name = product.Name,
                    DisplayName = !string.IsNullOrEmpty(product.DisplayName) ? product.DisplayName : product.Name,
                    Description = !string.IsNullOrEmpty(product.Description) ? product.Description : "",
                    Tags = !string.IsNullOrEmpty(product.Tags) ? product.Tags.Split(',').ToList() : new List<string>(),
                    Pricing = new PricingInfoV3
                    {
                        Amount = product.Price,
                        Currency = !string.IsNullOrEmpty(product.Currency) ? product.Currency : "USD",
                        Discounted = product.IsDiscounted,
                        DiscountedAmount = product.DiscountedPrice,
                        PriceHistory = new List<PriceHistoryEntryV3>()
                    },
                    Variants = new List<VariantInfoV3>()
                },
                Inventory = new InventoryInfoV3
                {
                    InStock = product.InStock,
                    Quantity = product.Quantity,
                    ReservedQuantity = 0,
                    Warehouse = new WarehouseInfoV3
                    {
                        Location = "Primary Warehouse",
                        Code = "WH-001"
                    },
                    InventoryHistory = new List<InventoryHistoryEntryV3>()
                },
                Analytics = new AnalyticsInfoV3
                {
                    Views = product.Views,
                    Purchases = product.Purchases,
                    Rating = product.Rating,
                    ReviewsCount = product.ReviewsCount,
                    TopReviews = new List<ReviewInfoV3>()
                },
                Category = new CategoryInfoV3
                {
                    PrimaryCategory = !string.IsNullOrEmpty(product.Category) ? product.Category : "General",
                    SubCategories = new List<string>(),
                    Department = !string.IsNullOrEmpty(product.Department) ? product.Department : "Default"
                }
            };

            logger.LogInformation("Retrieved product {ProductId} using V3 endpoint", id);
            return Ok(response);
        }
    }

    // POST - Create a new product - only available in v3
    [HttpPost]
    [MapToApiVersion("3.0")]
    public async Task<ActionResult<ProductResponseV3>> CreateProduct(CreateProductRequestV3 request)
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            DisplayName = request.DisplayName ?? request.Name,
            Description = request.Description ?? "",
            Price = request.Price,
            Currency = request.Currency ?? "USD",
            IsDiscounted = request.IsDiscounted,
            DiscountedPrice = request.DiscountedPrice,
            InStock = request.InStock,
            Quantity = request.Quantity,
            Category = request.Category ?? "General",
            Department = request.Department ?? "Default",
            Tags = string.Join(",", request.Tags),
            Views = 0,
            Purchases = 0,
            Rating = 0.0,
            ReviewsCount = 0
        };

        context.Products.Add(product);
        await context.SaveChangesAsync();

        var response = new ProductResponseV3
        {
            Id = product.Id,
            Product = new ProductInfoV3
            {
                Name = product.Name,
                DisplayName = product.DisplayName,
                Description = product.Description,
                Tags = product.Tags?.Split(',').ToList() ?? new List<string>(),
                Pricing = new PricingInfoV3
                {
                    Amount = product.Price,
                    Currency = product.Currency,
                    Discounted = product.IsDiscounted,
                    DiscountedAmount = product.DiscountedPrice,
                    PriceHistory = new List<PriceHistoryEntryV3>()
                },
                Variants = new List<VariantInfoV3>()
            },
            Inventory = new InventoryInfoV3
            {
                InStock = product.InStock,
                Quantity = product.Quantity,
                ReservedQuantity = 0,
                Warehouse = new WarehouseInfoV3
                {
                    Location = "Primary Warehouse",
                    Code = "WH-001"
                },
                InventoryHistory = new List<InventoryHistoryEntryV3>()
            },
            Analytics = new AnalyticsInfoV3
            {
                Views = product.Views,
                Purchases = product.Purchases,
                Rating = product.Rating,
                ReviewsCount = product.ReviewsCount,
                TopReviews = new List<ReviewInfoV3>()
            },
            Category = new CategoryInfoV3
            {
                PrimaryCategory = product.Category,
                SubCategories = new List<string>(),
                Department = product.Department
            }
        };

        logger.LogInformation("Created new product {ProductId}", product.Id);
        return CreatedAtAction(nameof(GetProduct), new { id = product.Id, version = "3.0" }, response);
    }

    // PUT - Update a product - only available in v3
    [HttpPut("{id:guid}")]
    [MapToApiVersion("3.0")]
    public async Task<IActionResult> UpdateProduct(Guid id, UpdateProductRequestV3 request)
    {
        var product = await context.Products.FindAsync(id);
        if (product is null)
        {
            return NotFound();
        }

        product.Name = request.Name ?? product.Name;
        product.DisplayName = request.DisplayName ?? product.DisplayName;
        product.Description = request.Description ?? product.Description;
        product.Price = request.Price ?? product.Price;
        product.Currency = request.Currency ?? product.Currency;
        product.IsDiscounted = request.IsDiscounted ?? product.IsDiscounted;
        product.DiscountedPrice = request.DiscountedPrice ?? product.DiscountedPrice;
        product.InStock = request.InStock ?? product.InStock;
        product.Quantity = request.Quantity ?? product.Quantity;
        product.Category = request.Category ?? product.Category;
        product.Department = request.Department ?? product.Department;
        product.Tags = request.Tags != null ? string.Join(",", request.Tags) : product.Tags;

        await context.SaveChangesAsync();

        logger.LogInformation("Updated product {ProductId}", id);
        return NoContent();
    }

    // DELETE - Remove a product - available in v2 and v3
    [HttpDelete("{id:guid}")]
    [MapToApiVersion("2.0")]
    [MapToApiVersion("3.0")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        var product = await context.Products.FindAsync(id);
        if (product is null)
        {
            return NotFound();
        }

        context.Products.Remove(product);
        await context.SaveChangesAsync();

        logger.LogInformation("Deleted product {ProductId}", id);
        return NoContent();
    }

    // GET analytics for a product - only available in v3
    [HttpGet("{id:guid}/analytics")]
    [MapToApiVersion("3.0")]
    public async Task<ActionResult<AnalyticsInfoV3>> GetProductAnalytics(Guid id)
    {
        var product = await context.Products.FindAsync(id);
        if (product is null)
        {
            return NotFound();
        }

        var analytics = new AnalyticsInfoV3
        {
            Views = product.Views,
            Purchases = product.Purchases,
            Rating = product.Rating,
            ReviewsCount = product.ReviewsCount,
            TopReviews = new List<ReviewInfoV3>()
        };

        return Ok(analytics);
    }

    // POST increase view count - only available in v3
    [HttpPost("{id:guid}/view")]
    [MapToApiVersion("3.0")]
    public async Task<IActionResult> RecordView(Guid id)
    {
        var product = await context.Products.FindAsync(id);
        if (product is null)
        {
            return NotFound();
        }

        product.Views++;
        await context.SaveChangesAsync();

        return NoContent();
    }
}
