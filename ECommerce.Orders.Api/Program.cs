using BuildingBlocks.OpenApi;
using BuildingBlocks.Web;
using ECommerce.Infrastructure.Extensions;
using ECommerce.Orders.Api.Features.Health;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddMinimalEndpoints(assemblies: typeof(Program).Assembly);
builder.Services.AddEndpointsApiExplorer();

// Configure OpenAPI without versioning
builder.Services.AddSwaggerGen();
builder.AddInfrastructure(typeof(Program).Assembly);

WebApplication app = builder.Build();

app.MapMinimalEndpoints();

// app.UseInfrastructure(); // This line triggers EF Core migrations which conflict with db-init
app.MapHealthEndpoints();

// Configure Swagger UI without versioning
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Orders API V1");
});

app.Run();

namespace ECommerce.Orders.Api
{
    // For tests
    public partial class Program
    {
    }
}
