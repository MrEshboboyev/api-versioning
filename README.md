# 🚀 API Versioning and Feature Flags in ASP.NET Core  

This repository demonstrates how to implement **API versioning**, **feature flags**, and **feature flag targeting** in **ASP.NET Core**. It uses **Entity Framework Core (EF Core)** with an in-memory database for practical examples of managing dynamic feature control and versioning strategies.  

## 🌟 Features  

### Core Concepts  
- **API Versioning**: Handle multiple versions of your API for backward compatibility.  
- **Feature Flags**: Toggle features on or off without redeploying code.  
- **Feature Flag Targeting**: Customize feature availability based on user segments, environments, or conditions.  

### Tools and Libraries  
- **ASP.NET Core**: Build a scalable and modular API.  
- **EF Core with In-Memory DB**: Simulates real-world database operations in memory for development and testing.  

## 📂 Repository Structure  

```
📦 src  
 ┣ 📂 Products.Api    # Products.Api ASP.NET Core project  
```  

## 🛠 Getting Started  

### Prerequisites  
Ensure you have the following installed:  
- .NET Core SDK  
- A modern C# IDE (e.g., Visual Studio or JetBrains Rider)  

### Step 1: Clone the Repository  
```bash  
git clone https://github.com/MrEshboboyev/api-versioning.git  
cd api-versioning  
```  

### Step 2: Run the Project  
```bash  
dotnet run --project src/Products.Api 
```  

### Step 3: Test the Endpoints  
Use a tool like Postman or Curl to interact with the API.  

## 🌐 API Endpoints  

### Example: Versioned Endpoints  
| Method | Endpoint                       | API Version | Description                       |  
|--------|--------------------------------|-------------|-----------------------------------|  
| GET    | `/api/v1/products`             | v1          | Retrieve products (v1 version)   |  
| GET    | `/api/v2/products/{id:guid}`   | v2          | Retrieve product (v2 version)   |  
| GET    | `/api/v1/products/{id:guid}`   | v1          | Retrieve product (v1 version)   |  

## 📖 Code Highlights  

### API Versioning Implementation  
```csharp  
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
}
```  

### Feature Flags Example  
```csharp  
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
```  

## 🧪 Testing  
The repository includes unit and integration tests to ensure feature flags and versioning work as expected.  

### Example Test Case  
```csharp  
[Fact]  
public void ToggleFeature_ShouldUpdateFlagState()  
{  
    // Arrange  
    var service = new FeatureFlagService();  

    // Act  
    service.ToggleFeature("NewFeature", true);  

    // Assert  
    Assert.True(service.IsFeatureActive("NewFeature"));  
}  
```  

## 🌟 Why This Project?  
1. **Backward Compatibility**: Learn how to manage multiple API versions effectively.  
2. **Dynamic Features**: Control application behavior without redeployment using feature flags.  
3. **Scalable Design**: Build APIs with maintainable and extensible patterns.  
4. **Real-World Examples**: Hands-on demonstration of EF Core with in-memory DB.  

## 🏗 About the Author  
This project was developed by [MrEshboboyev](https://github.com/MrEshboboyev), who is passionate about building scalable, maintainable, and high-quality software solutions.  

## 📄 License  
This project is licensed under the MIT License. Feel free to use and adapt the code for your own projects.  

## 🔖 Tags  
C#, ASP.NET Core, API Versioning, Feature Flags, Feature Targeting, EF Core, In-Memory Database, Backend Development, Software Architecture, Feature Management  

---  

Feel free to suggest additional features or ask questions! 🚀  
