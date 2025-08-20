using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Compression;
using MongoDB.Driver.Core.Configuration;
using MongoDB.Driver.Core.Events;
using STC.ProductCatalog.Domain.Aggregates.Products.Repositories;
using STC.ProductCatalog.Persistence.MongoDb._Shared.UnitOfWorks;
using STC.ProductCatalog.Persistence.MongoDb.Collections.Products.Repositories;
using STC.ProductCatalog.Persistence.MongoDb.Contexts;

namespace STC.ProductCatalog.Persistence.MongoDb;

public static class DependencyInjection
{
    public static IServiceCollection AddMongoDbPersistenceDependencies(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<MongoClient>(_ =>
        {
            MongoClientSettings settings = MongoClientSettings.FromConnectionString(
                connectionString: configuration.GetConnectionString(name: "MongoDb") ??
                                  throw new ArgumentNullException(
                                      message: "MongoDb connection string is not configured.", innerException: null));

            settings.ApplicationName = "STC.ProductCatalog";

            settings.MaxConnectionPoolSize = Environment.ProcessorCount * 20;
            settings.MinConnectionPoolSize = Environment.ProcessorCount * 2;
            settings.MaxConnectionIdleTime = TimeSpan.FromMinutes(5);

            settings.RetryReads = true;
            settings.RetryWrites = true;

            settings.WriteConcern = WriteConcern.WMajority;
            settings.ReadConcern = ReadConcern.Local;
            settings.Compressors =
            [
                new CompressorConfiguration(CompressorType.Snappy)
            ];

            return new MongoClient(settings: settings);
        });

        services.AddScoped<IMongoDbContext, MongoDbContext>();
        services.AddScoped<IClientSessionHandle>(provider => provider.GetService<MongoClient>()!.StartSession());
        services.AddScoped<IUnitOfWork, MongoDbUnitOfWork>();

        services.AddScoped<IProductReadRepository, MongoDbProductReadRepository>();
        services.AddScoped<IProductWriteRepository, MongoDbProductWriteRepository>();

        return services;
    }
}