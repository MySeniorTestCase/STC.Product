namespace STC.ProductCatalog.Application.Features.Products.Notifications.ProductCreationRequest;

public class
    ProductCreationRequestNotificationRequestValidator : AbstractValidator<ProductCreationRequestNotificationRequest>
{
    public ProductCreationRequestNotificationRequestValidator()
    {
        RuleFor(x => x.Name).NotNull();
        RuleFor(x => x.Description).NotNull();
        RuleFor(x => x.Image).NotNull();
        RuleFor(x => x.Price).NotNull();
    }
}