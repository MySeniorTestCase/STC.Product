using Scalar.AspNetCore;
using STC.ProductCatalog.Application;
using STC.ProductCatalog.Domain;
using STC.ProductCatalog.Infrastructure;
using STC.ProductCatalog.Persistence.MongoDb;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddAntiforgery();
builder.Services.AddControllers();

builder.Services.AddDomainDependencies()
    .AddApplicationDependencies()
    .AddInfrastructureDependencies(configuration: builder.Configuration)
    .AddMongoDbPersistenceDependencies(configuration: builder.Configuration);

WebApplication app = builder.Build();

app.MapControllers();
app.UseAntiforgery();
app.MapOpenApi();
app.MapScalarApiReference();

app.Run();