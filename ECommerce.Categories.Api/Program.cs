//using BuildingBlocks.OpenApi;
//using BuildingBlocks.Web;
//using ECommerce;
//using ECommerce.Extensions;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;

//WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

//builder.AddMinimalEndpoints(assemblies: typeof(EcommerceRoot).Assembly);
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddCustomVersioning();
//builder.Services.AddAspnetOpenApi();
//builder.AddInfrastructure();

//WebApplication app = builder.Build();

//app.MapMinimalEndpoints();
//app.UseInfrastructure();

//if (!app.Environment.IsProduction())
//{
//    _ = app.UseAspnetOpenApi();
//}

//app.Run();

//namespace ECommerce.Api
//{
//    // For tests
//    public partial class Programh
//    {
//    }
//}
