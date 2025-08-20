using STC.ProductCatalog.Application.Utilities.Pagination;

namespace STC.ProductCatalog.Application.Features.Products.Queries.GetProducts;

public record GetProductsQueryRequest(PaginationRequest Pagination)
    : IRequest<IDataResponse<GetProductsQueryResponse[]>>;