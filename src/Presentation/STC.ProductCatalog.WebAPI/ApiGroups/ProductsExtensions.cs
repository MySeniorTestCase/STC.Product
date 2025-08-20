using MediatR;
using Microsoft.AspNetCore.Mvc;
using STC.ProductCatalog.Application.Features.Products.Notifications.ProductCreationRequest;

namespace STC.ProductCatalog.WebAPI.ApiGroups;

public static class ProductsExtensions
{
    public static void MapProductsApi(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(prefix: "/api/products").WithTags("Products");

        group.MapPost("/",
                async ([FromForm] ProductCreationRequestNotificationRequest notification, IMediator mediator,
                    CancellationToken cancellationToken) =>
                {
                    await mediator.Publish(notification, cancellationToken: cancellationToken);
                    return Results.Accepted();
                })
            .WithName("Create Product Request");
    }
}