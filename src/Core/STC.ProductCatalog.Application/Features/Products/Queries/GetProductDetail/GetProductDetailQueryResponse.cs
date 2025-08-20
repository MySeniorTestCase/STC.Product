namespace STC.ProductCatalog.Application.Features.Products.Queries.GetProductDetail;

public record GetProductDetailQueryResponse(string Id, string Name, string Description, string ImageUrl, decimal Price);