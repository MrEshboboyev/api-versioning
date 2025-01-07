using Microsoft.EntityFrameworkCore;
using Products.Api.Data;
using Products.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<ProductsDbContext>(options =>
{
    options.UseInMemoryDatabase("ProductsDb");
});

builder.Services.AddScoped<ProductSeeder>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.SeedDatabaseAsync();

await app.RunAsync();

app.Run();