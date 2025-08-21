using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using STC.ProductCatalog.Application.Features.Products.Constants;
using STC.ProductCatalog.Application.Features.Products.Helpers;
using STC.ProductCatalog.Application.Utilities.ObjectStorage;
using STC.ProductCatalog.Application.Utilities.Prices;
using STC.ProductCatalog.Application.Utilities.Responses.Concretes;
using STC.ProductCatalog.Domain.Aggregates.Products;
using STC.ProductCatalog.Domain.Aggregates.Products.Repositories;

namespace STC.ProductCatalog.Application.Features.Products.Queries.GetProducts;

public class GetProductsQueryRequestHandler(
    HybridCache cache,
    IProductReadRepository productReadRepository,
    IEnumerable<IObjectStorageService> objectStorageServices,
    ILogger<GetProductsQueryRequestHandler> logger)
    : IRequestHandler<GetProductsQueryRequest, IDataResponse<GetProductsQueryResponse[]>>
{
    private async Task<DataResponseBase<GetProductsQueryResponse[]>> GetProductsAsync(
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching products from database.");

        ICollection<Product> products = await productReadRepository.GetAllAsync(cancellationToken: cancellationToken);

        logger.LogInformation(message: "Fetched {Count} products from database.", products.Count);

        var result = ResponseCreator.Success(message: string.Empty,
            data: products.Select(_product => new GetProductsQueryResponse(id: _product.Id,
                name: _product.Name,
                description: _product.Description,
                imageUrl: objectStorageServices.Select(_objectStorageService =>
                                  _objectStorageService.MergeUrl(media: _product.Medias.FirstOrDefault()))
                              .FirstOrDefault(_mediaUrl => string.IsNullOrEmpty(_mediaUrl) is false) ??
                          ProductConstants.DefaultImageUrl,
                price: PriceHelper.ToDollar(_product.Price))
            ).ToArray());

        return ResponseCreator.Convert<GetProductsQueryResponse[]>(result);
    }

    public async Task<IDataResponse<GetProductsQueryResponse[]>> Handle(GetProductsQueryRequest request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Get Products request received.");

        var result = await cache.GetOrCreateAsync<DataResponseBase<GetProductsQueryResponse[]>>(
            key: ProductCacheKeyHelper.ProductsCacheKey,
            factory: async ctk => await GetProductsAsync(cancellationToken: ctk),
            cancellationToken: cancellationToken);

        logger.LogInformation(message: "Get Products request completed. Total products: {Count}", result.Data!.Length);

        return result;
    }
}