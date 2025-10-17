namespace Products.Api.Data;

public class ProductSeeder(ProductsDbContext context)
{
    public void Seed()
    {
        if (context.Products.Any()) return;
        var products = new List<Product>
        {
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Product A",
                Price = 10.99m,
                DisplayName = "Product A",
                Currency = "USD",
                IsDiscounted = false,
                InStock = true,
                Quantity = 100,
                Description = "A high-quality product A",
                Tags = "electronics,tech,gadget",
                Category = "Electronics",
                Department = "Technology",
                Views = 150,
                Purchases = 25,
                Rating = 4.5,
                ReviewsCount = 12,
                DiscountedPrice = null
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Product B",
                Price = 15.49m,
                DisplayName = "Product B",
                Currency = "USD",
                IsDiscounted = true,
                InStock = true,
                Quantity = 200,
                Description = "An improved version of product B",
                Tags = "home,utility,improved",
                Category = "Home",
                Department = "Household",
                Views = 320,
                Purchases = 87,
                Rating = 4.2,
                ReviewsCount = 34,
                DiscountedPrice = 12.99m
            }
        };

        context.Products.AddRange(products);
        context.SaveChanges();
    }
}

public sealed class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public string Currency { get; set; } = string.Empty;
    public bool IsDiscounted { get; set; }
    public bool InStock { get; set; }
    public int Quantity { get; set; }
    
    // V3 additional fields
    public string? Description { get; set; }
    public string? Tags { get; set; }
    public string? Category { get; set; }
    public string? Department { get; set; }
    public int Views { get; set; }
    public int Purchases { get; set; }
    public double Rating { get; set; }
    public int ReviewsCount { get; set; }
    public decimal? DiscountedPrice { get; set; }
}
