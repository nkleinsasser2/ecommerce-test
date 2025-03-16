using BuildingBlocks.OpenApi;
using BuildingBlocks.Web;
using ECommerce.Infrastructure.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddMinimalEndpoints(assemblies: typeof(Program).Assembly);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCustomVersioning();
builder.Services.AddAspnetOpenApi();
builder.AddInfrastructure(typeof(Program).Assembly);

WebApplication app = builder.Build();

app.MapMinimalEndpoints();
app.UseInfrastructure();

_ = app.UseAspnetOpenApi();

app.Run();

namespace ECommerce.Categories.Api
{
    // For tests
    public partial class Programh
    {
    }
}
