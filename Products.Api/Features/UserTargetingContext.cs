using Microsoft.FeatureManagement.FeatureFilters;

namespace Products.Api.Features;

public class UserTargetingContext(
    IHttpContextAccessor httpContextAccessor) : ITargetingContextAccessor
{
    private const string CacheKey = "UserTargetingContext.TargetingContext";
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public ValueTask<TargetingContext> GetContextAsync()
    {
        var httpContext = _httpContextAccessor.HttpContext!;

        if (httpContext.Items.TryGetValue(CacheKey, out object? value))
        {
            return new ValueTask<TargetingContext>((TargetingContext)value!);
        }

        var targetingContext = new TargetingContext
        {
            UserId = GetUserId(httpContext),
            Groups = GetUserGroups(httpContext)
        };
        
        return new ValueTask<TargetingContext>(targetingContext);
    }

    private static string GetUserId(HttpContext? httpContext)
    {
        // For demo purposes, we'll check for a user ID in the headers
        // In a real app, this might come from authentication claims
        return httpContext?.Request.Headers["X-User-Id"].FirstOrDefault() ?? string.Empty;
    }

    private static string[] GetUserGroups(HttpContext? httpContext)
    {
        // In a real app, these might come from claims or a database
        var userGroups = httpContext?.Request.Headers["X-User-Groups"]
                             .FirstOrDefault()?
                             .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                         ?? [];

        return userGroups;
    }
}