using Microsoft.Extensions.DependencyInjection;
using Products.Api.Data;

namespace Products.Api.Extensions;

public static class WebApplicationExtensions
{
    public static async Task<WebApplication> SeedDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<ProductsDbContext>();
        await context.Database.EnsureCreatedAsync();

        var seeder = services.GetRequiredService<ProductSeeder>();
        seeder.Seed();

        return app;
    }
}