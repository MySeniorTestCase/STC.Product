using Scalar.AspNetCore;
using STC.ProductCatalog.Application;
using STC.ProductCatalog.Domain;
using STC.ProductCatalog.Infrastructure;
using STC.ProductCatalog.Persistence.MongoDb;
using STC.ProductCatalog.WebAPI.ApiGroups;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddAntiforgery();

builder.Services.AddDomainDependencies()
    .AddApplicationDependencies()
    .AddInfrastructureDependencies(configuration: builder.Configuration)
    .AddMongoDbPersistenceDependencies(configuration: builder.Configuration);

WebApplication app = builder.Build();

app.UseAntiforgery();
app.MapOpenApi();
app.MapScalarApiReference();
app.MapProductsApi();

app.Run();