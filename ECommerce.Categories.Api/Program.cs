using BuildingBlocks.OpenApi;
using BuildingBlocks.Web;
using ECommerce.Categories.Api.Features.Health;
using ECommerce.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddMinimalEndpoints(assemblies: typeof(Program).Assembly);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCustomVersioning();
builder.Services.AddAspnetOpenApi();
builder.AddInfrastructure(typeof(Program).Assembly);

WebApplication app = builder.Build();

app.MapMinimalEndpoints();
app.UseInfrastructure();

// Map health check endpoint
app.MapHealthEndpoints();

_ = app.UseAspnetOpenApi();

// Use the configured URLs from environment variables instead of hardcoding
app.Run();

namespace ECommerce.Categories.Api
{
    // For tests
    public partial class Program
    {
    }
}
