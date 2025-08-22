using Scalar.AspNetCore;
using STC.ProductCatalog.Application;
using STC.ProductCatalog.Domain;
using STC.ProductCatalog.Infrastructure;
using STC.ProductCatalog.Persistence.MongoDb;
using STC.ProductCatalog.WebAPI;
using STC.ProductCatalog.WebAPI.Middlewares;
using STC.Shared.Auth.Consumer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddAntiforgery();
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<GlobalExceptionHandlerMiddleware>();
builder.Services.AddAuthentication(defaultScheme: CustomAuthenticationOptions.DefaultScheme)
    .AddScheme<CustomAuthenticationOptions, CustomAuthenticationHandler>(CustomAuthenticationOptions.DefaultScheme,
        displayName: null, configureOptions: null);

builder.Services.AddAuthorization(configure: authorizationOptions =>
{
    authorizationOptions.AddPolicy(name: AuthConstants.Policies.CanUpdateProductPolicyName,
        configurePolicy: policy => policy.RequireAssertion(handler: (context) =>
        {
            if (context.User.Identity is null || context.User.Identity.IsAuthenticated is false) return false;

            return context.User.IsInRole(role: AuthConstants.Roles.Admin) ||
                   context.User.IsInRole(role: AuthConstants.Roles.Moderator);
        }));
});

builder.Services.AddDomainDependencies()
    .AddApplicationDependencies()
    .AddInfrastructureDependencies(configuration: builder.Configuration, loggingBuilder: builder.Logging)
    .AddMongoDbPersistenceDependencies(configuration: builder.Configuration);

WebApplication app = builder.Build();

app.MapOpenApi();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
app.MapControllers();
app.UseAntiforgery();
app.MapScalarApiReference(_ => _.Servers = []);

app.Run();