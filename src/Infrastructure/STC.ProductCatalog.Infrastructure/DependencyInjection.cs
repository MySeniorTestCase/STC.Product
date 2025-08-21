using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using STC.ProductCatalog.Application.Utilities.DomainEventDispatcher;
using STC.ProductCatalog.Application.Utilities.ObjectStorage;
using STC.ProductCatalog.Domain._Shared.Events;
using STC.ProductCatalog.Infrastructure.Constants;
using STC.ProductCatalog.Infrastructure.Features.Products.Consumers.ProductCreation;
using STC.ProductCatalog.Infrastructure.Features.Products.Consumers.ProductUpdate;
using STC.ProductCatalog.Infrastructure.Utilities.DomainEventDispatcher;
using STC.ProductCatalog.Infrastructure.Utilities.ObjectStorages;

namespace STC.ProductCatalog.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<IDomainEventDispatcher, MediatorDomainEventDispatcher>();
        services.AddSingleton<IObjectStorageService, LocalObjectStorageManager>();

        services.AddHybridCache();

        services.AddStackExchangeRedisCache(setupAction: options => options.Configuration =
            configuration.GetConnectionString("Redis") ??
            throw new InvalidOperationException(
                message: "Redis connection string is not configured.",
                innerException: null));

        services.AddMassTransit(x =>
        {
            x.AddConsumer<ProductCreationConsumer>();
            x.AddConsumer<ProductUpdateConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(hostAddress: new Uri(configuration.GetConnectionString("RabbitMQ") ??
                                              throw new InvalidOperationException(
                                                  message: "RabbitMQ connection string is not configured.",
                                                  innerException: null)));

                cfg.ReceiveEndpoint(queueName: QueueInformations.ProductCreationRequestQueue,
                    e => e.ConfigureConsumer<ProductCreationConsumer>(context));
                
                cfg.ReceiveEndpoint(queueName: QueueInformations.ProductUpdateRequestQueue,
                    e => e.ConfigureConsumer<ProductUpdateConsumer>(context));

                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}