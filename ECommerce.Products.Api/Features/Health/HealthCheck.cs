using BuildingBlocks.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ECommerce.Products.Api.Features.Health;

public static class HealthEndpoints
{
    public static WebApplication MapHealthEndpoints(this WebApplication app)
    {
        app.MapGet("/health", HealthCheck)
            .AllowAnonymous()
            .WithTags("Health");
        
        return app;
    }

    private static IResult HealthCheck(ILogger<Program> logger)
    {
        logger.LogInformation("Health check performed");
        return Results.Ok(new { status = "Healthy", service = "Products API" });
    }
} 