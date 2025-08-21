using MediatR;
using Microsoft.AspNetCore.Mvc;
using STC.ProductCatalog.Application.Features.Products.Commands.CreateProduct;
using STC.ProductCatalog.Application.Features.Products.Commands.UpdateProduct;
using STC.ProductCatalog.Application.Features.Products.Notifications.ProductCreationRequest;
using STC.ProductCatalog.Application.Features.Products.Notifications.ProductUpdateRequest;
using STC.ProductCatalog.Application.Features.Products.Queries.GetProductDetail;
using STC.ProductCatalog.Application.Features.Products.Queries.GetProducts;

namespace STC.ProductCatalog.WebAPI.Controllers;

[ApiController, Route("api/products")]
public class ProductsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IResult> GetProductsAsync([FromQuery] GetProductsQueryRequest request,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(request, cancellationToken);
        return new ResponseGenerator(result);
    }

    [HttpGet("{id}")]
    public async Task<IResult> GetProductDetailAsync([FromRoute] string id,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetProductDetailQueryRequest(Id: id), cancellationToken);
        return new ResponseGenerator(result);
    }

    [HttpPost]
    public async Task<IResult> CreateProductAsync([FromForm] ProductCreationRequestNotificationRequest request,
        CancellationToken cancellationToken)
    {
        await mediator.Publish(request, cancellationToken);
        return Results.Accepted();
    }

    [HttpPut]
    public async Task<IResult> UpdateProductAsync([FromForm] ProductUpdateRequestNotificationRequest request,
        CancellationToken cancellationToken)
    {
        await mediator.Publish(request, cancellationToken);
        return Results.Accepted();
    }
}