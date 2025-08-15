using E_Commerce.ApiGateway.Extensions;
using E_Commerce.ApiGateway.Middleware;
using E_Commerce.Common.Api.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Logging
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

// Services
builder.Services.AddAuthentication(builder.Configuration);
builder.Services.AddCaching(builder.Configuration);
builder.Services.AddRateLimiting(builder.Configuration);
builder.Services.AddReverseProxy(builder.Configuration);
builder.Services.AddHealthChecks();
builder.Services.AddSwaggerDocumentation();

var app = builder.Build();

// Configure pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "E-Commerce API Gateway v1");
        c.RoutePrefix = "swagger";
    });
}

// Security and middleware
app.UseMiddleware<SecurityHeadersMiddleware>();
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseSerilogRequestLogging();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<TenantMiddleware>();
app.UseMiddleware<RateLimitingMiddleware>();

// Health Checks
app.MapHealthChecks("/health");

// YARP Reverse Proxy
app.MapReverseProxy();

app.Run();
