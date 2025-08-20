using Microsoft.Extensions.Caching.Hybrid;
using STC.ProductCatalog.Application.Features.Products.Helpers;
using STC.ProductCatalog.Application.Utilities.Responses.Concretes;
using STC.ProductCatalog.Domain.Aggregates.Products.Repositories;

namespace STC.ProductCatalog.Application.Features.Products.Queries.GetProducts;

public class
    GetProductsQueryRequestHandler(HybridCache cache, IProductReadRepository productReadRepository)
    : IRequestHandler<GetProductsQueryRequest, IDataResponse<GetProductsQueryResponse[]>>
{
    private async Task<DataResponseBase<GetProductsQueryResponse[]>> GetProductsAsync(
        CancellationToken cancellationToken)
    {
        var products = await productReadRepository.GetAllAsync(cancellationToken: cancellationToken);

        var result = ResponseCreator.Success(message: string.Empty,
            data: products.Select(_product => new GetProductsQueryResponse(id: _product.Id)).ToArray());

        return ResponseCreator.Convert<GetProductsQueryResponse[]>(result);
    }

    public async Task<IDataResponse<GetProductsQueryResponse[]>> Handle(GetProductsQueryRequest request,
        CancellationToken cancellationToken)
    {
        return await cache.GetOrCreateAsync<DataResponseBase<GetProductsQueryResponse[]>>(
            key: ProductCacheKeyHelper.ProductsCacheKey,
            factory: async cancel => await GetProductsAsync(cancellationToken: cancellationToken),
            cancellationToken: cancellationToken);
    }
}