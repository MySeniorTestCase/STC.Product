namespace STC.ProductCatalog.Application.Features.Products.Commands.DeleteProduct;

public class DeleteProductCommandRequestValidator : AbstractValidator<DeleteProductCommandRequest>
{
    public DeleteProductCommandRequestValidator()
    {
        RuleFor(x => x.Id).NotNull().NotEmpty();
    }
}