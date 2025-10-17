using Microsoft.Extensions.Primitives;

namespace Products.Api.Middleware;

public class ApiVersionMiddleware(
    RequestDelegate next,
    ILogger<ApiVersionMiddleware> logger
)
{
    public async Task InvokeAsync(HttpContext context)
    {
        // Extract API version from request
        var apiVersion = context.GetRequestedApiVersion();
        
        if (apiVersion != null)
        {
            // Add version information to response headers
            context.Response.Headers.Append("X-API-Version", $"{apiVersion.MajorVersion}.{apiVersion.MinorVersion}");
            
            // Log version-specific request information
            logger.LogInformation("Processing request for API version {ApiVersion}", apiVersion);
            
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

        // Call the next middleware in the pipeline
        await next(context);
    }

    private async Task HandleV1Request(HttpContext context)
    {
        // V1-specific processing
        logger.LogInformation("Applying V1-specific processing");
        
        // Add V1-specific headers
        context.Response.Headers.Append("X-Version-Features", "basic");
        
        // Simulate V1-specific behavior
        await Task.CompletedTask;
    }

    private async Task HandleV2Request(HttpContext context)
    {
        // V2-specific processing
        logger.LogInformation("Applying V2-specific processing");
        
        // Add V2-specific headers
        context.Response.Headers.Append("X-Version-Features", "enhanced");
        
        // Simulate V2-specific behavior
        await Task.CompletedTask;
    }

    private async Task HandleV3Request(HttpContext context)
    {
        // V3-specific processing
        logger.LogInformation("Applying V3-specific processing");
        
        // Add V3-specific headers
        context.Response.Headers.Append("X-Version-Features", "advanced");
        
        // Simulate V3-specific behavior
        await Task.CompletedTask;
    }
}

// Extension method for easier registration
public static class ApiVersionMiddlewareExtensions
{
    public static IApplicationBuilder UseApiVersionMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ApiVersionMiddleware>();
    }
}
