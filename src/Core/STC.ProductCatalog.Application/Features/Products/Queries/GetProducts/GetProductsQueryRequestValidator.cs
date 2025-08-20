namespace STC.ProductCatalog.Application.Features.Products.Queries.GetProducts;

public class GetProductsQueryRequestValidator : AbstractValidator<GetProductsQueryRequest>
{
    public GetProductsQueryRequestValidator()
    {
        // RuleFor(x => x.Pagination).NotNull().NotEmpty();
    }
}