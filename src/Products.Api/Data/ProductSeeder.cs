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
                Quantity = 100
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
                Quantity = 200
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
}