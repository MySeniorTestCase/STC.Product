using MassTransit;
using Microsoft.Extensions.Logging;
using STC.ProductCatalog.Application.Features.Products.Notifications.ProductCreationRequest;
using STC.ProductCatalog.Application.Utilities.Logging;
using STC.ProductCatalog.Application.Utilities.ObjectStorage;
using STC.ProductCatalog.Application.Utilities.ObjectStorage.Models;
using STC.ProductCatalog.Domain.Aggregates.Products;
using STC.ProductCatalog.Infrastructure.Constants;
using STC.ProductCatalog.Infrastructure.Features.Products.Consumers.ProductCreation;

namespace STC.ProductCatalog.Infrastructure.Features.Products.Notifications.ProductCreationRequest;

public class
    ProductCreationRequestNotificationRequestHandler(
        IObjectStorageService objectStorageService,
        ISendEndpointProvider sendEndpointProvider,
        ILogger<ProductCreationRequestNotificationRequestHandler> logger)
    : INotificationHandler<ProductCreationRequestNotificationRequest>
{
    public async Task Handle(ProductCreationRequestNotificationRequest request, CancellationToken cancellationToken)
    {
        string maskedProductName = request.Name.ToMaskedString(takeHalf: true);
        logger.LogInformation(message: "Received product creation request: {0}", maskedProductName);

        var validateResult =
            Product.PreValidate(name: request.Name, description: request.Description, price: request.Price);
        if (validateResult.IsValid is false)
        {
            string errors = string.Join(", ", validateResult.Errors);

            logger.LogWarning(message: "Product is not valid. Errors: {0}", errors);

            throw new ArgumentException(message: errors);
        }

        IDataResponse<ObjectStorageUploadResult> imageUploadResult =
            await objectStorageService.UploadAsync(file: request.Image, cancellationToken: cancellationToken);
        if (imageUploadResult.IsSuccess is false)
        {
            logger.LogCritical(message: "Image could not be uploaded. Error: {0}", imageUploadResult.Message);

            throw new InvalidOperationException(message: Messages.ProductImageUploadFailed);
        }

        ISendEndpoint sendEndpoint =
            await sendEndpointProvider.GetSendEndpoint(address: QueueInformations.ProductCreationRequestQueueUri);

        await sendEndpoint.Send(message: new ProductCreationMessage(Name: request.Name,
                Description: request.Description,
                Image: imageUploadResult.Data!,
                Price: request.Price),
            cancellationToken: cancellationToken);
        
        logger.LogInformation(message: "Product creation request sent successfully to queue. Name: {0}", maskedProductName);
    }
}