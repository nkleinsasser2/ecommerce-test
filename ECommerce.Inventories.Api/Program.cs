using BuildingBlocks.OpenApi;
using BuildingBlocks.Web;
using ECommerce.Infrastructure.Extensions;
using ECommerce.Inventories.Api.Features.Health;
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
// app.UseInfrastructure(); // This line triggers EF Core migrations which conflict with db-init

// Map health check endpoint
app.MapHealthEndpoints();

_ = app.UseAspnetOpenApi();

// Use the configured URLs from environment variables instead of hardcoding
app.Run();

namespace ECommerce.Inventories.Api
{
    // For tests
    public partial class Program
    {
    }
}
