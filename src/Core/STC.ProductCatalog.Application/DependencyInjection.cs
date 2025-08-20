using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using STC.ProductCatalog.Application.BehaviorPipelines.Validation;

namespace STC.ProductCatalog.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
    {
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        services.AddMediatR(_ => _.RegisterServicesFromAssemblies(assemblies: assemblies));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviorPipeline<,>));
        services.AddValidatorsFromAssemblies(assemblies: assemblies);
        return services;
    }
}