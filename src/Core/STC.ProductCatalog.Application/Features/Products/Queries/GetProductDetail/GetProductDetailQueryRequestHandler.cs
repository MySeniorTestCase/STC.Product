using Microsoft.Extensions.Caching.Hybrid;
using STC.ProductCatalog.Application.Features.Products.Helpers;
using STC.ProductCatalog.Application.Utilities.Responses.Concretes;
using STC.ProductCatalog.Domain.Aggregates.Products;
using STC.ProductCatalog.Domain.Aggregates.Products.Repositories;

namespace STC.ProductCatalog.Application.Features.Products.Queries.GetProductDetail;

public class GetProductDetailQueryRequestHandler(IProductReadRepository productReadRepository, HybridCache cache)
    : IRequestHandler<GetProductDetailQueryRequest, IDataResponse<GetProductDetailQueryResponse>>
{
    private async ValueTask<DataResponseBase<GetProductDetailQueryResponse>> GetProductDetailAsync(
        GetProductDetailQueryRequest request, CancellationToken cancellationToken)
    {
        Product? product = await productReadRepository.GetAsync(exp: _product => _product.Id == request.Id,
            cancellationToken: cancellationToken);
        if (product is null)
            return ResponseCreator.Convert(
                ResponseCreator.Error<GetProductDetailQueryResponse>(message: Messages.ProductIsNotFound, data: null));

        var result = ResponseCreator.Success(message: string.Empty, data: new GetProductDetailQueryResponse(
            Id: product.Id,
            Name: product.Name,
            Description: product.Description,
            ImageUrl: product.ImageUrl,
            Price: product.Price));

        return ResponseCreator.Convert(result);
    }

    public async Task<IDataResponse<GetProductDetailQueryResponse>> Handle(GetProductDetailQueryRequest request,
        CancellationToken cancellationToken)
    {
        return await cache.GetOrCreateAsync<DataResponseBase<GetProductDetailQueryResponse>>(
            key: ProductCacheKeyHelper.GetProductDetailCacheKey(productId: request.Id),
            factory: async ctk => await GetProductDetailAsync(request: request, cancellationToken: ctk),
            cancellationToken: cancellationToken);
    }
}