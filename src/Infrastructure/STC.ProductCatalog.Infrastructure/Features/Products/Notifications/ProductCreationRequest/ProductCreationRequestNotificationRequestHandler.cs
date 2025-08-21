using MassTransit;
using STC.ProductCatalog.Application.Features.Products.Notifications.ProductCreationRequest;
using STC.ProductCatalog.Application.Utilities.ObjectStorage;
using STC.ProductCatalog.Domain.Aggregates.Products;
using STC.ProductCatalog.Infrastructure.Constants;
using STC.ProductCatalog.Infrastructure.Features.Products.Consumers.ProductCreation;

namespace STC.ProductCatalog.Infrastructure.Features.Products.Notifications.ProductCreationRequest;

public class
    ProductCreationRequestNotificationRequestHandler(
        IObjectStorageService objectStorageService,
        ISendEndpointProvider sendEndpointProvider)
    : INotificationHandler<ProductCreationRequestNotificationRequest>
{
    public async Task Handle(ProductCreationRequestNotificationRequest request, CancellationToken cancellationToken)
    {
        var validateResult =
            Product.PreValidate(name: request.Name, description: request.Description, price: request.Price);
        if (validateResult.IsValid is false)
        {
            string errors = string.Join(", ", validateResult.Errors);
            throw new ArgumentException(message: errors);
        }

        IDataResponse<string> imageUploadResult =
            await objectStorageService.UploadAsync(file: request.Image, cancellationToken: cancellationToken);
        if (imageUploadResult.IsSuccess is false)
            throw new InvalidOperationException(message: Messages.ProductImageUploadFailed);

        ISendEndpoint sendEndpoint =
            await sendEndpointProvider.GetSendEndpoint(address: QueueInformations.ProductCreationRequestQueueUri);

        await sendEndpoint.Send(message: new ProductCreationMessage(Name: request.Name,
                Description: request.Description,
                ImageUrl: imageUploadResult.Data!,
                Price: request.Price),
            cancellationToken: cancellationToken);
    }
}