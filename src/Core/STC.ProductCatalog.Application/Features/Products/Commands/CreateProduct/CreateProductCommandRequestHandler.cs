using System.Net;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using STC.ProductCatalog.Application.Features.Products.Helpers;
using STC.ProductCatalog.Application.Utilities.DomainEventDispatcher;
using STC.ProductCatalog.Application.Utilities.Logging;
using STC.ProductCatalog.Domain._Shared.Medias;
using STC.ProductCatalog.Domain.Aggregates.Products;
using STC.ProductCatalog.Domain.Aggregates.Products.Repositories;
using STC.ProductCatalog.Domain.Aggregates.Products.Services;

namespace STC.ProductCatalog.Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommandRequestHandler(
    IProductDomainService productDomainService,
    IProductWriteRepository productWriteRepository,
    IDomainEventDispatcher domainEventDispatcher,
    ILogger<CreateProductCommandRequestHandler> logger,
    HybridCache cache)
    : IRequestHandler<CreateProductCommandRequest, IDataResponse<CreateProductCommandResponse>>
{
    public async Task<IDataResponse<CreateProductCommandResponse>> Handle(CreateProductCommandRequest request,
        CancellationToken cancellationToken)
    {
        string maskedProductName = request.Name.ToMaskedString(takeHalf: true);
        logger.LogInformation(message: "A product creation started. Name: {0}", maskedProductName);

        Product product = await productDomainService.CreateAsync(name: request.Name,
            description: request.Description,
            media: Media.Create(provider: request.Image.Provider, fileName: request.Image.FileName),
            price: request.Price,
            cancellationToken: cancellationToken);

        await productWriteRepository.CreateAsync(entity: product, cancellationToken: cancellationToken);

        if (await productWriteRepository.SaveChangesAsync(cancellationToken: cancellationToken) is false)
        {
            logger.LogCritical(message: "A product could not be created to the database. Name: {0}", maskedProductName);
            throw new InvalidOperationException(message: Messages.ProductCouldNotBeCreated);
        }

        await cache.RemoveAsync(key: ProductCacheKeyHelper.ProductsCacheKey, cancellationToken: cancellationToken);

        await domainEventDispatcher.DispatchAsync(product, cancellationToken: cancellationToken);

        logger.LogInformation(message: "A product creation done. Name: {0}", maskedProductName);

        return ResponseCreator.Success(message: Messages.ProductCreatedSuccessfully,
            data: new CreateProductCommandResponse(Id: product.Id),
            statusCode: HttpStatusCode.Created);
    }
}