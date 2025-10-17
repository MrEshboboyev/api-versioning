using Asp.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.FeatureManagement;
using Products.Api.Data;
using Products.Api.Extensions;
using Products.Api.Features;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<ProductsDbContext>(options =>
{
    options.UseInMemoryDatabase("ProductsDb");
});

builder.Services.AddScoped<ProductSeeder>();

// add API versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
})
.AddMvc()
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddFeatureManagement()
    .WithTargeting<UserTargetingContext>();

// Add NSwag services
builder.Services.AddSwaggerDocument(config =>
    config.PostProcess = (settings =>
    {
        settings.Info.Title = "Products API";
        settings.Info.Version = "v1";
        settings.Info.Description = "An API for managing products with versioning and feature flags.";
    }));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Use NSwag middleware
    app.UseOpenApi();
    app.UseSwaggerUi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.SeedDatabaseAsync();

await app.RunAsync();

app.Run();