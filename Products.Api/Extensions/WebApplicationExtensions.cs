namespace Products.Api.Extensions;

public static class WebApplicationExtensions
{
    public static async Task<WebApplication> SeedDatabaseAsync(this WebApplication app)
    {
        return app;
    }
}