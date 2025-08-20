namespace STC.ProductCatalog.Application.Features.Products.Helpers;

public static class ProductCacheKeyHelper
{
    private const string ProductDetailCacheKey = "Products:Detail:{0}";
    public static string GetProductDetailCacheKey(string productId) => string.Format(ProductDetailCacheKey, productId);

    public const string ProductsCacheKey = "Products:List";
}