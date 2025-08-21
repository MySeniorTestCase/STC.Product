using MassTransit;
using STC.ProductCatalog.Application.Features.Products.Notifications.ProductUpdateRequest;
using STC.ProductCatalog.Application.Utilities.ObjectStorage;
using STC.ProductCatalog.Domain.Aggregates.Products;
using STC.ProductCatalog.Domain.Aggregates.Products.Repositories;
using STC.ProductCatalog.Infrastructure.Constants;
using STC.ProductCatalog.Infrastructure.Features.Products.Consumers.ProductUpdate;

namespace STC.ProductCatalog.Infrastructure.Features.Products.Notifications.ProductUpdateRequest;

public class ProductUpdateRequestNotificationRequestHandler(
    IProductReadRepository productReadRepository,
    IObjectStorageService objectStorageService,
    ISendEndpointProvider sendEndpointProvider)
    : INotificationHandler<ProductUpdateRequestNotificationRequest>
{
    public async Task Handle(ProductUpdateRequestNotificationRequest request, CancellationToken cancellationToken)
    {
        var preValidateResult = Product.PreValidate(name: request.Name,
            description: request.Description,
            price: request.Price);
        if (preValidateResult.IsValid is false)
        {
            string errors = string.Join(", ", preValidateResult.Errors);

            throw new ArgumentException(message: errors);
        }

        bool isProductNotFound = await productReadRepository.AnyAsync(exp: _product => _product.Id == request.Id,
            cancellationToken: cancellationToken) is false;
        if (isProductNotFound)
            throw new ArgumentNullException(message: Messages.ProductIsNotFound, innerException: null);

        string? uploadedImageUrl = null;
        if (request.Image is not null)
        {
            var uploadResult =
                await objectStorageService.UploadAsync(file: request.Image, cancellationToken: cancellationToken);
            if (uploadResult.IsSuccess is false)
                throw new InvalidOperationException(message: Messages.ProductImageUploadFailed);

            uploadedImageUrl = uploadResult.Data;
        }

        ISendEndpoint sendEndpoint =
            await sendEndpointProvider.GetSendEndpoint(address: QueueInformations.ProductUpdateRequestQueueUri);

        await sendEndpoint.Send(message: new ProductUpdateMessage(Id: request.Id,
                Name: request.Name,
                Description: request.Description,
                NewImageUrl: uploadedImageUrl,
                Price: request.Price),
            cancellationToken: cancellationToken);
    }
}