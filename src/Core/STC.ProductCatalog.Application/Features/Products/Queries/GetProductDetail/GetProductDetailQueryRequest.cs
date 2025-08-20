namespace STC.ProductCatalog.Application.Features.Products.Queries.GetProductDetail;

public record GetProductDetailQueryRequest(string Id) : IRequest<IDataResponse<GetProductDetailQueryResponse>>;