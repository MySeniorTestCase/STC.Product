using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using STC.ProductCatalog.Application.Features.Products.Helpers;
using STC.ProductCatalog.Application.Utilities.DomainEventDispatcher;
using STC.ProductCatalog.Application.Utilities.Logging;
using STC.ProductCatalog.Application.Utilities.ObjectStorage;
using STC.ProductCatalog.Domain._Shared.Medias;
using STC.ProductCatalog.Domain.Aggregates.Products;
using STC.ProductCatalog.Domain.Aggregates.Products.Repositories;
using STC.ProductCatalog.Domain.Aggregates.Products.Services;

namespace STC.ProductCatalog.Application.Features.Products.Commands.UpdateProduct;

public class UpdateProductCommandRequestHandler(
    IProductReadRepository productReadRepository,
    IProductWriteRepository productWriteRepository,
    IProductDomainService productDomainService,
    IDomainEventDispatcher domainEventDispatcher,
    IObjectStorageService objectStorageService,
    ILogger<UpdateProductCommandRequestHandler> logger,
    HybridCache cache) : IRequestHandler<UpdateProductCommandRequest, IResponse>
{
    public async Task<IResponse> Handle(UpdateProductCommandRequest request, CancellationToken cancellationToken)
    {
        string maskedId = request.Id.ToMaskedString(takeHalf: true);

        Product? product = await productReadRepository.GetAsync(exp: _product => _product.Id == request.Id,
            cancellationToken: cancellationToken);
        if (product is null)
        {
            logger.LogWarning(message: "Product not found in database. Id: {0}", maskedId);

            return ResponseCreator.Error(message: Messages.ProductIsNotFound);
        }

        product.SetDescription(description: request.Description);
        product.SetPrice(price: request.Price);

        if (request.NewImage is not null)
            product.AddMedia(Media.Create(provider: request.NewImage.Provider, fileName: request.NewImage.FileName));

        await productDomainService.SetNameAsync(product: product, name: request.Name,
            cancellationToken: cancellationToken);

        await productWriteRepository.UpdateAsync(entity: product, cancellationToken: cancellationToken);

        if (await productWriteRepository.SaveChangesAsync(cancellationToken: cancellationToken) is false)
        {
            logger.LogCritical(message: "Product could not be updated. Id: {0}", maskedId);
            throw new InvalidOperationException(message: Messages.ProductCouldNotBeUpdated);
        }

        bool isImageUrlChanged = request.NewImage is not null;
        if (isImageUrlChanged)
        {
            string[] oldImageUrlsToRemove = product.Medias
                .Where(_media => _media.FileName != request.NewImage!.FileName)
                .Select(_fileName => _fileName.FileName)
                .Distinct().ToArray();

            IResponse deleteResult = await objectStorageService.DeleteAsync(fileNames: oldImageUrlsToRemove,
                cancellationToken: cancellationToken);
            if (deleteResult.IsSuccess is false)
                logger.LogWarning(
                    message: "Image could not be deleted from object storage. Error Message: {0}, Id: {2}",
                    deleteResult.Message, maskedId);
        }

        await cache.RemoveAsync(
            keys:
            [
                ProductCacheKeyHelper.ProductsCacheKey,
                ProductCacheKeyHelper.GetProductDetailCacheKey(productId: request.Id)
            ], cancellationToken: cancellationToken);

        await domainEventDispatcher.DispatchAsync(product, cancellationToken: cancellationToken);

        logger.LogInformation(message: "Product updated successfully. Id: {0}", maskedId);

        return ResponseCreator.Success(message: Messages.ProductUpdatedSuccessfully);
    }
}