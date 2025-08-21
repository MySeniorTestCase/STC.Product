namespace STC.ProductCatalog.Application.Features.Products.Notifications.ProductUpdateRequest;

public class
    ProductUpdateRequestNotificationRequestValidator : AbstractValidator<ProductUpdateRequestNotificationRequest>
{
    public ProductUpdateRequestNotificationRequestValidator()
    {
        RuleFor(x => x.Id).NotNull().NotEmpty();
        RuleFor(x => x.Name).NotNull();
        RuleFor(x => x.Description).NotNull();
        RuleFor(x => x.Image);
        RuleFor(x => x.Price).NotNull();
    }
}