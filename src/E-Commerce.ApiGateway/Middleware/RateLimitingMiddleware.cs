using AspNetCoreRateLimit;

namespace E_Commerce.ApiGateway.Middleware;

public class RateLimitingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IpRateLimitMiddleware _ipRateLimitMiddleware;

    public RateLimitingMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
    {
        _next = next;
        _ipRateLimitMiddleware = new IpRateLimitMiddleware(next, 
            serviceProvider.GetRequiredService<IProcessingStrategy>(),
            serviceProvider.GetRequiredService<IOptions<IpRateLimitOptions>>(),
            serviceProvider.GetRequiredService<IMemoryCache>(),
            serviceProvider.GetRequiredService<IRateLimitCounterStore>(),
            serviceProvider.GetRequiredService<IConfiguration>(),
            serviceProvider.GetRequiredService<ILogger<IpRateLimitMiddleware>>());
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await _ipRateLimitMiddleware.InvokeAsync(context);
    }
}
