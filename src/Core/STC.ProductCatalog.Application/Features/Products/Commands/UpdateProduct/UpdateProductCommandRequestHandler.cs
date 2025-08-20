using Microsoft.Extensions.Caching.Hybrid;
using STC.ProductCatalog.Application.Features.Products.Helpers;
using STC.ProductCatalog.Domain.Aggregates.Products;
using STC.ProductCatalog.Domain.Aggregates.Products.Repositories;
using STC.ProductCatalog.Domain.Aggregates.Products.Services;

namespace STC.ProductCatalog.Application.Features.Products.Commands.UpdateProduct;

public class UpdateProductCommandRequestHandler(
    IProductReadRepository productReadRepository,
    IProductWriteRepository productWriteRepository,
    IProductDomainService productDomainService,
    HybridCache cache) : IRequestHandler<UpdateProductCommandRequest, IResponse>
{
    public async Task<IResponse> Handle(UpdateProductCommandRequest request, CancellationToken cancellationToken)
    {
        Product? product = await productReadRepository.GetAsync(exp: _product => _product.Id == request.Id,
            cancellationToken: cancellationToken);
        if (product is null)
            return ResponseCreator.Error(message: Messages.ProductIsNotFound);

        await productDomainService.SetNameAsync(product: product, name: request.Name,
            cancellationToken: cancellationToken);

        await productWriteRepository.UpdateAsync(entity: product, cancellationToken: cancellationToken);

        if (await productWriteRepository.SaveChangesAsync(cancellationToken: cancellationToken) is false)
            throw new InvalidOperationException(message: Messages.ProductCouldNotBeUpdated);

        await cache.RemoveAsync(
            keys:
            [
                ProductCacheKeyHelper.ProductsCacheKey,
                ProductCacheKeyHelper.GetProductDetailCacheKey(productId: request.Id)
            ], cancellationToken: cancellationToken);

        return ResponseCreator.Success(message: Messages.ProductUpdatedSuccessfully);
    }
}