namespace STC.ProductCatalog.Application.Features.Products.Queries.GetProducts;

public record GetProductsQueryRequest : IRequest<IDataResponse<GetProductsQueryResponse[]>>;