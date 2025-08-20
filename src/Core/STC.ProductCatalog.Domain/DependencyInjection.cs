using Microsoft.Extensions.DependencyInjection;
using STC.ProductCatalog.Domain.Aggregates.Products.Services;

namespace STC.ProductCatalog.Domain;

public static class DependencyInjection
{
    public static IServiceCollection AddDomainDependencies(this IServiceCollection services)
    {
        services.AddScoped<IProductDomainService, ProductDomainManager>();
        return services;
    }
}