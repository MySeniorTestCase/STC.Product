using System.Net;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using STC.ProductCatalog.Application.Features.Products.Commands.DeleteProduct;
using STC.ProductCatalog.Application.Features.Products.Notifications.ProductCreationRequest;
using STC.ProductCatalog.Application.Features.Products.Notifications.ProductUpdateRequest;
using STC.ProductCatalog.Application.Features.Products.Queries.GetProductDetail;
using STC.ProductCatalog.Application.Features.Products.Queries.GetProducts;
using STC.ProductCatalog.Domain.Constants;
using STC.Shared.Utilities.Response;

namespace STC.ProductCatalog.WebAPI.Controllers;

[ApiController, Route("products")]
public class ProductsController(IMediator mediator) : ControllerBase
{
    private const int MultipartBodyLengthLimit = 5 * (1024 * 1024); // 5MB limit

    [HttpGet, AllowAnonymous]
    public async Task<IResult> GetProductsAsync([FromQuery] GetProductsQueryRequest request,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(request, cancellationToken);
        return new ResponseGenerator(result);
    }

    [HttpGet("{id}"), AllowAnonymous]
    public async Task<IResult> GetProductDetailAsync([FromRoute] string id,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetProductDetailQueryRequest(Id: id), cancellationToken);
        return new ResponseGenerator(result);
    }

    [HttpPost, Authorize(Roles = AuthConstants.Roles.Admin), RequestFormLimits(MultipartBodyLengthLimit = MultipartBodyLengthLimit)]
    public async Task<IResult> CreateProductAsync([FromForm] ProductCreationRequestNotificationRequest request,
        CancellationToken cancellationToken)
    {
        await mediator.Publish(request, cancellationToken);
        return new ResponseGenerator(ResponseCreator.Success(message: Messages.YourProductCreationRequestHasBeenQueued,
            statusCode: HttpStatusCode.Accepted));
    }

    [HttpPut, Authorize(Policy = AuthConstants.Policies.CanUpdateProductPolicyName),
     RequestFormLimits(MultipartBodyLengthLimit = MultipartBodyLengthLimit)]
    public async Task<IResult> UpdateProductAsync([FromForm] ProductUpdateRequestNotificationRequest request,
        CancellationToken cancellationToken)
    {
        await mediator.Publish(request, cancellationToken);
        return new ResponseGenerator(ResponseCreator.Success(message: Messages.YourProductUpdateRequestHasBeenQueued,
            statusCode: HttpStatusCode.Accepted));
    }

    [HttpDelete("{id}"), Authorize(Roles = AuthConstants.Roles.Admin)]
    public async Task<IResult> DeleteProductAsync([FromRoute] string id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteProductCommandRequest(Id: id), cancellationToken);

        return new ResponseGenerator(result);
    }
}