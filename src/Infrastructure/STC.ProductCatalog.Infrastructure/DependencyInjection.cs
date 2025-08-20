using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using STC.ProductCatalog.Application.Utilities.ObjectStorage;
using STC.ProductCatalog.Infrastructure.Constants;
using STC.ProductCatalog.Infrastructure.Features.Products.Consumers.ProductCreation;
using STC.ProductCatalog.Infrastructure.Utilities.ObjectStorages;

namespace STC.ProductCatalog.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<IObjectStorageService, LocalObjectStorageManager>();

        services.AddMassTransit(x =>
        {
            x.AddConsumer<ProductCreationConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(hostAddress: new Uri(configuration.GetConnectionString("RabbitMQ") ??
                                              throw new ArgumentNullException(
                                                  message: "RabbitMQ connection string is not configured.",
                                                  innerException: null)));

                cfg.ReceiveEndpoint(queueName: QueueInformations.ProductCreationRequestQueue,
                    e => { e.ConfigureConsumer<ProductCreationConsumer>(context); });

                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}