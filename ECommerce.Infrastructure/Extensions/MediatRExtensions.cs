
using System.Reflection;
using BuildingBlocks.EFCore;
using BuildingBlocks.Logging;
using BuildingBlocks.Validation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Infrastructure.Extensions;
public static class MediatRExtensions
{
    public static IServiceCollection AddCustomMediatR(this IServiceCollection services, params Assembly[] assemblies)
    {
        _ = services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies));
        _ = services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        _ = services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        _ = services.AddTransient(typeof(IPipelineBehavior<,>), typeof(EfTxBehavior<,>));

        return services;
    }
}
