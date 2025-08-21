using Scalar.AspNetCore;
using STC.ProductCatalog.Application;
using STC.ProductCatalog.Domain;
using STC.ProductCatalog.Infrastructure;
using STC.ProductCatalog.Persistence.MongoDb;
using STC.ProductCatalog.WebAPI.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddAntiforgery();
builder.Services.AddControllers();
builder.Services.AddScoped<GlobalExceptionHandlerMiddleware>();

builder.Services.AddDomainDependencies()
    .AddApplicationDependencies()
    .AddInfrastructureDependencies(configuration: builder.Configuration, loggingBuilder: builder.Logging)
    .AddMongoDbPersistenceDependencies(configuration: builder.Configuration);

WebApplication app = builder.Build();

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
app.MapControllers();
app.UseAntiforgery();
app.MapOpenApi();
app.MapScalarApiReference();

app.Run();