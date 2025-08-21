using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using STC.ProductCatalog.Application.Features.Products.Constants;
using STC.ProductCatalog.Application.Features.Products.Helpers;
using STC.ProductCatalog.Application.Utilities.Logging;
using STC.ProductCatalog.Application.Utilities.ObjectStorage;
using STC.ProductCatalog.Domain.Aggregates.Products;
using STC.ProductCatalog.Domain.Aggregates.Products.Repositories;
using STC.Shared.Utilities.Response.Concretes;

namespace STC.ProductCatalog.Application.Features.Products.Queries.GetProductDetail;

public class GetProductDetailQueryRequestHandler(
    IProductReadRepository productReadRepository,
    HybridCache cache,
    ILogger<GetProductDetailQueryRequestHandler> logger,
    IEnumerable<IObjectStorageService> objectStorageServices)
    : IRequestHandler<GetProductDetailQueryRequest, IDataResponse<GetProductDetailQueryResponse>>
{
    private string _maskedProductId = string.Empty;

    private async ValueTask<DataResponseBase<GetProductDetailQueryResponse>> GetProductDetailAsync(
        GetProductDetailQueryRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation(message: "Fetching product details from database for product ID: {ProductId}",
            _maskedProductId);

        Product? product = await productReadRepository.GetAsync(exp: _product => _product.Id == request.Id,
            cancellationToken: cancellationToken);
        if (product is null)
        {
            logger.LogWarning("Product with ID {ProductId} not found", _maskedProductId);
            return ResponseCreator.Convert(
                ResponseCreator.Error<GetProductDetailQueryResponse>(message: Messages.ProductIsNotFound, data: null));
        }

        string imageUrl = objectStorageServices
            .Select(_objectStorageService => _objectStorageService.MergeUrl(media: product.Medias.FirstOrDefault()))
            .FirstOrDefault(_mediaUrl => string.IsNullOrEmpty(_mediaUrl) is false) ?? ProductConstants.DefaultImageUrl;

        var result = ResponseCreator.Success(message: string.Empty, data: new GetProductDetailQueryResponse(
            Id: product.Id,
            Name: product.Name,
            Description: product.Description,
            ImageUrl: imageUrl,
            Price: product.Price,
            CreatedAt: product.CreatedAt));

        logger.LogInformation("Product details fetched from database for product ID: {ProductId}", _maskedProductId);

        return ResponseCreator.Convert(result);
    }

    public async Task<IDataResponse<GetProductDetailQueryResponse>> Handle(GetProductDetailQueryRequest request,
        CancellationToken cancellationToken)
    {
        _maskedProductId = request.Id.ToMaskedString(takeHalf: true);

        logger.LogInformation(message: "Product detail request received for product ID: {ProductId}", _maskedProductId);

        var result = await cache.GetOrCreateAsync<DataResponseBase<GetProductDetailQueryResponse>>(
            key: ProductCacheKeyHelper.GetProductDetailCacheKey(productId: request.Id),
            factory: async ctk => await GetProductDetailAsync(request: request, cancellationToken: ctk),
            cancellationToken: cancellationToken);

        logger.LogInformation(message: "Product detail request completed for product ID: {ProductId}",
            _maskedProductId);

        return result;
    }
}