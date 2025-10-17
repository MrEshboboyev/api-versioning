# 🚀 Advanced API Versioning and Feature Flags in ASP.NET Core  

This repository demonstrates how to implement **advanced API versioning**, **feature flags**, and **feature flag targeting** in **ASP.NET Core**. It showcases the true potential and power of API versioning with comprehensive examples including multiple versions, version-specific endpoints, middleware, and documentation.  

## 🌟 Features  

### Core Concepts  
- **Advanced API Versioning**: Handle multiple versions of your API with sophisticated versioning strategies  
- **Feature Flags**: Toggle features on or off without redeploying code  
- **Feature Flag Targeting**: Customize feature availability based on user segments, environments, or conditions  
- **Version-Specific Middleware**: Custom processing logic based on API version  
- **API Documentation**: Auto-generated API documentation using NSwag with version-specific documentation  

### Tools and Libraries  
- **ASP.NET Core**: Build a scalable and modular API  
- **EF Core with In-Memory DB**: Simulates real-world database operations in memory for development and testing  
- **NSwag**: Generates Swagger/OpenAPI documentation and client code  
- **Asp.Versioning.Mvc**: Advanced API versioning library for ASP.NET Core  

## 📂 Repository Structure  

```
📦 src  
 ┣ 📂 Products.Api    # Products.Api ASP.NET Core project  
      ┣ 📂 Controllers  # Versioned API controllers  
      ┣ 📂 Data         # Database context and seeding  
      ┣ 📂 Extensions   # Extension methods  
      ┣ 📂 Features     # Feature flags and targeting  
      ┣ 📂 Middleware   # Version-specific middleware  
      ┣ 📂 Models       # Versioned response models  
```  

## 🛠 Getting Started  

### Prerequisites  
Ensure you have the following installed:  
- .NET 9.0 SDK  
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

### Step 3: Access API Documentation  
Once the application is running, you can access the auto-generated API documentation at:  
- Swagger UI: `http://localhost:5076/swagger`  

### Step 4: Test the Endpoints  
Use a tool like Postman or Curl to interact with the API.  

## 🌐 API Endpoints  

### Versioned Endpoints Overview  
The API now supports three distinct versions (v1, v2, v3) with increasingly sophisticated features:  

| Version | Features | Description |  
|---------|----------|-------------|  
| v1 | Basic product info | Simple product name and price |  
| v2 | Enhanced product info | Structured product data with inventory tracking |  
| v3 | Advanced product info | Full product analytics, categorization, and variants |  

### Detailed Endpoint Matrix  
| Method | Endpoint | API Version | Description |  
|--------|----------|-------------|-------------|  
| GET | `/api/v{version}/products` | v1, v2, v3 | Retrieve all products (version-specific response) |  
| GET | `/api/v{version}/products/{id}` | v1, v2, v3 | Retrieve specific product (version-specific response) |  
| POST | `/api/v3/products` | v3 only | Create new product with advanced properties |  
| PUT | `/api/v3/products/{id}` | v3 only | Update product with advanced properties |  
| DELETE | `/api/v{version}/products/{id}` | v2, v3 | Delete a product |  
| GET | `/api/v3/products/{id}/analytics` | v3 only | Get product analytics data |  
| POST | `/api/v3/products/{id}/view` | v3 only | Record product view for analytics |  

### Version Selection Methods  
The API supports multiple methods for specifying the API version:  
1. **URL Segment**: `/api/v3/products`  
2. **Query String**: `/api/products?api-version=3.0`  
3. **HTTP Header**: `X-Version: 3.0`  
4. **Media Type**: `Accept: application/json; ver=3.0`  

## 📖 Code Highlights  

### Advanced API Versioning Implementation  
```csharp  
[ApiController]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
[ApiVersion("3.0")]
[Route("api/v{version:apiVersion}/products")]
public sealed class ProductsController : ControllerBase
{
    // Version-agnostic endpoint that returns different responses based on version
    [HttpGet]
    public async Task<ActionResult> GetProducts(ApiVersion apiVersion)
    {
        return apiVersion.MajorVersion switch
        {
            1 => await GetProductsV1(),
            2 => await GetProductsV2(),
            3 => await GetProductsV3(),
            _ => await GetProductsV3()
        };
    }
}
```  

### Version-Specific Middleware  
```csharp  
public class ApiVersionMiddleware
{
    public async Task InvokeAsync(HttpContext context, ApiVersioningOptions apiVersioningOptions)
    {
        var apiVersion = context.GetRequestedApiVersion();
        
        if (apiVersion != null)
        {
            // Add version information to response headers
            context.Response.Headers.Append("X-API-Version", $"{apiVersion.MajorVersion}.{apiVersion.MinorVersion}");
            
            // Apply version-specific logic
            switch (apiVersion.MajorVersion)
            {
                case 1:
                    await HandleV1Request(context);
                    break;
                case 2:
                    await HandleV2Request(context);
                    break;
                case 3:
                    await HandleV3Request(context);
                    break;
            }
        }
        await _next(context);
    }
}
```  

### Feature Flags with Versioning  
```csharp  
[HttpGet("{id:guid}")]
[MapToApiVersion("2.0")]
[FeatureGate(FeatureFlags.UseV2ProductApi)]
public async Task<ActionResult<ProductResponseV2>> GetProductV2(Guid id)
{
    if (!await featureManager.IsEnabledAsync(FeatureFlags.UseV2ProductApi))
    {
        return NotFound();
    }
    // Implementation...
}
```  

## 📚 API Documentation with NSwag  

This project includes enhanced NSwag integration for automatic API documentation generation with version-specific documentation:  

### Features  
- **Multi-Version Documentation**: Separate documentation for each API version (v1, v2, v3)  
- **Swagger UI**: Interactive API exploration with version switching  
- **OpenAPI Specification**: Machine-readable API specifications for each version  
- **Client Code Generation**: Generate client SDKs for different platforms  

### Accessing Documentation  
1. Run the application using `dotnet run --project src/Products.Api`  
2. Navigate to `http://localhost:5076/swagger` in your browser  
3. Switch between API versions using the version selector  

## 🧪 Testing  
The repository includes unit and integration tests to ensure feature flags and versioning work as expected.  

### Example Test Case  
```csharp  
[Theory]
[InlineData("1.0")]
[InlineData("2.0")]
[InlineData("3.0")]
public async Task GetProducts_ReturnsCorrectVersionData(string version)
{
    // Arrange
    var client = _factory.CreateClient();
    
    // Act
    var response = await client.GetAsync($"/api/v{version}/products");
    
    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.OK);
    // Additional version-specific assertions...
}
```  

## 🌟 Why This Project?  
1. **Advanced Versioning**: Learn sophisticated API versioning strategies beyond basic implementations  
2. **Backward Compatibility**: Manage multiple API versions effectively with proper deprecation strategies  
3. **Dynamic Features**: Control application behavior without redeployment using feature flags  
4. **Scalable Design**: Build APIs with maintainable and extensible patterns  
5. **Real-World Examples**: Hands-on demonstration of EF Core with in-memory DB  
6. **API Documentation**: Auto-generated, version-specific documentation for easier API consumption  
7. **Middleware Processing**: Custom processing logic based on API version  

## 🏗 About the Author  
This project was developed by [MrEshboboyev](https://github.com/MrEshboboyev), who is passionate about building scalable, maintainable, and high-quality software solutions.  

## 📄 License  
This project is licensed under the MIT License. Feel free to use and adapt the code for your own projects.  

## 🔖 Tags  
C#, ASP.NET Core, API Versioning, Feature Flags, Feature Targeting, EF Core, In-Memory Database, Backend Development, Software Architecture, Feature Management, NSwag, Swagger, OpenAPI, Middleware, Versioning Strategies  

---  

Feel free to suggest additional features or ask questions! 🚀