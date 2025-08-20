namespace STC.ProductCatalog.Application.Features.Products.Queries.GetProductDetail;

public class GetProductDetailQueryRequestValidator : AbstractValidator<GetProductDetailQueryRequest>
{
    public GetProductDetailQueryRequestValidator()
    {
        RuleFor(x => x.Id).NotNull().NotEmpty();
    }
}