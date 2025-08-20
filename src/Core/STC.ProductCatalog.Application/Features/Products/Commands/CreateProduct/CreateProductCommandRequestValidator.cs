namespace STC.ProductCatalog.Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommandRequestValidator : AbstractValidator<CreateProductCommandRequest>
{
    public CreateProductCommandRequestValidator()
    {
        RuleFor(x => x.Name).NotNull();
        RuleFor(x => x.Description).NotNull();
        RuleFor(x => x.ImageUrl).NotNull();
        RuleFor(x => x.Price).NotNull();
    }
}