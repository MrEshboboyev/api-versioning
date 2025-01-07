using Microsoft.EntityFrameworkCore;
using Products.Api.Models;

namespace Products.Api.Data;

public class ProductsDbContext(DbContextOptions<ProductsDbContext> options) : DbContext(options)
{
    public DbSet<ProductResponseV1> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductResponseV1>().ToTable("products");
    }
}