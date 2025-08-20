namespace STC.ProductCatalog.Application.Features.Products.Commands.UpdateProduct;

public class UpdateProductCommandRequestValidator : AbstractValidator<UpdateProductCommandRequest>
{
    public UpdateProductCommandRequestValidator()
    {
        RuleFor(x => x.Id).NotNull().NotEmpty();
        RuleFor(x => x.Name).NotNull();
        RuleFor(x => x.Description).NotNull();
        RuleFor(x => x.Image);
        RuleFor(x => x.Price).NotNull();
    }
}