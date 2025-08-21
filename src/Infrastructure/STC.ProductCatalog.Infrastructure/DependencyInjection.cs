using System.Reflection;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Core;
using Serilog.Sinks.RabbitMQ;
using STC.ProductCatalog.Application.Utilities.DomainEventDispatcher;
using STC.ProductCatalog.Application.Utilities.ObjectStorage;
using STC.ProductCatalog.Domain._Shared.Events;
using STC.ProductCatalog.Infrastructure.Constants;
using STC.ProductCatalog.Infrastructure.Features.Products.Consumers.ProductCreation;
using STC.ProductCatalog.Infrastructure.Features.Products.Consumers.ProductUpdate;
using STC.ProductCatalog.Infrastructure.Utilities.DomainEventDispatcher;
using STC.ProductCatalog.Infrastructure.Utilities.ObjectStorages;
using STC.Shared.Logger;

namespace STC.ProductCatalog.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services,
        IConfiguration configuration, ILoggingBuilder loggingBuilder)
    {
        {
            services.AddLoggerDependencies(loggingBuilder: loggingBuilder,
                options: _loggerOpt =>
                {
                    var loggerSect = configuration.GetRequiredSection("Logger");
                    var rabbitMqSect = loggerSect.GetRequiredSection("RabbitMq");

                    _loggerOpt.ApplicationName = Assembly.GetExecutingAssembly().FullName!;
                    _loggerOpt.ExchangeName = loggerSect["ExchangeName"]!;
                    _loggerOpt.BatchPostingLimit = int.Parse(loggerSect["BatchPostingLimit"]!);
                    _loggerOpt.RabbitMqOptions = new RabbitMqLoggerOptions()
                    {
                        HostName = rabbitMqSect["HostName"]!,
                        Port = int.Parse(rabbitMqSect["Port"]!),
                        UserName = rabbitMqSect["UserName"]!,
                        Password = rabbitMqSect["Password"]!
                    };
                });
        }

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