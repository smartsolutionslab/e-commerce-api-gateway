using System.Security.Claims;

namespace E_Commerce.ApiGateway.Middleware;

public class TenantMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<TenantMiddleware> _logger;

    public TenantMiddleware(RequestDelegate next, ILogger<TenantMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var tenantId = ExtractTenantId(context);
        
        if (!string.IsNullOrEmpty(tenantId))
        {
            context.Request.Headers["X-Tenant-Id"] = tenantId;
            _logger.LogInformation("Tenant ID {TenantId} extracted for request {RequestPath}", 
                tenantId, context.Request.Path);
        }

        await _next(context);
    }

    private string? ExtractTenantId(HttpContext context)
    {
        // Try to get tenant from JWT claims
        var tenantClaim = context.User?.FindFirst("tenant_id")?.Value;
        if (!string.IsNullOrEmpty(tenantClaim))
            return tenantClaim;

        // Try to get tenant from header
        var tenantHeader = context.Request.Headers["X-Tenant-Id"].FirstOrDefault();
        if (!string.IsNullOrEmpty(tenantHeader))
            return tenantHeader;

        // Try to get tenant from subdomain
        var host = context.Request.Host.Host;
        if (host.Contains('.'))
        {
            var subdomain = host.Split('.')[0];
            if (subdomain != "www" && subdomain != "api")
                return subdomain;
        }

        return null;
    }
}
