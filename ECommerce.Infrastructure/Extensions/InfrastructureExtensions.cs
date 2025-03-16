
using System.Reflection;
using BuildingBlocks.EFCore;
using BuildingBlocks.Logging;
using BuildingBlocks.Web;
using ECommerce.Infrastructure.Data;
using ECommerce.Infrastructure.Data.Seed;
using Figgle;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Sieve.Services;

namespace ECommerce.Infrastructure.Extensions;
public static class InfrastructureExtensions
{
    public static WebApplicationBuilder AddInfrastructure(this WebApplicationBuilder builder, Assembly assembly)
    {
        Microsoft.AspNetCore.Hosting.IWebHostEnvironment env = builder.Environment;

        AppOptions appOptions = builder.Services.GetOptions<AppOptions>(nameof(AppOptions));
        Console.WriteLine(FiggleFonts.Standard.Render(appOptions.Name));

        _ = builder.Services.AddProblemDetails();
        _ = builder.AddCustomSerilog(env);
        _ = builder.Services.AddScoped<ISieveProcessor, SieveProcessor>();
        _ = builder.Services.AddCustomMediatR(assembly);
        _ = builder.Services.AddValidatorsFromAssembly(assembly);
        _ = builder.Services.AddAutoMapper(assembly);
        _ = builder.Services.AddCustomDbContext<ECommerceDbContext>();
        _ = builder.Services.AddScoped<IDataSeeder, ECommerceDataSeeder>();

        return builder;
    }


    public static WebApplication UseInfrastructure(this WebApplication app)
    {
        Microsoft.AspNetCore.Hosting.IWebHostEnvironment env = app.Environment;
        AppOptions appOptions = app.GetOptions<AppOptions>(nameof(AppOptions));

        _ = app.UseCustomProblemDetails();

        _ = app.UseSerilogRequestLogging(options =>
        {
            options.EnrichDiagnosticContext = LogEnrichHelper.EnrichFromRequest;
        });

        _ = app.UseMigration<ECommerceDbContext>();

        _ = app.MapGet("/", x => x.Response.WriteAsync(appOptions.Name));

        return app;
    }
}
