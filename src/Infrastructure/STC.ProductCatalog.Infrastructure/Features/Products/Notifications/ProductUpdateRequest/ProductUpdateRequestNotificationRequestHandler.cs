using MassTransit;
using Microsoft.Extensions.Logging;
using STC.ProductCatalog.Application.Features.Products.Notifications.ProductUpdateRequest;
using STC.ProductCatalog.Application.Utilities.Logging;
using STC.ProductCatalog.Application.Utilities.ObjectStorage;
using STC.ProductCatalog.Application.Utilities.ObjectStorage.Models;
using STC.ProductCatalog.Domain.Aggregates.Products;
using STC.ProductCatalog.Domain.Aggregates.Products.Repositories;
using STC.ProductCatalog.Infrastructure.Constants;
using STC.ProductCatalog.Infrastructure.Features.Products.Consumers.ProductUpdate;

namespace STC.ProductCatalog.Infrastructure.Features.Products.Notifications.ProductUpdateRequest;

public class ProductUpdateRequestNotificationRequestHandler(
    IProductReadRepository productReadRepository,
    IObjectStorageService objectStorageService,
    ISendEndpointProvider sendEndpointProvider,
    ILogger<ProductUpdateRequestNotificationRequestHandler> logger)
    : INotificationHandler<ProductUpdateRequestNotificationRequest>
{
    public async Task Handle(ProductUpdateRequestNotificationRequest request, CancellationToken cancellationToken)
    {
        string maskedProductId = request.Id.ToMaskedString(takeHalf: true);

        logger.LogInformation(message: "Product update request received. Id: {0}", maskedProductId);

        var preValidateResult = Product.PreValidate(name: request.Name,
            description: request.Description,
            price: request.Price);
        if (preValidateResult.IsValid is false)
        {
            string errors = string.Join(", ", preValidateResult.Errors);

            logger.LogWarning(message: "Product update request validation failed. Id: {0}, Errors: {1}", maskedProductId,
                errors);

            throw new ArgumentException(message: errors);
        }

        bool isProductNotFound = await productReadRepository.AnyAsync(exp: _product => _product.Id == request.Id,
            cancellationToken: cancellationToken) is false;
        if (isProductNotFound)
        {
            logger.LogWarning(message: "Product not found for update. Id: {0}", maskedProductId);

            throw new ArgumentNullException(message: Messages.ProductIsNotFound, innerException: null);
        }

        ObjectStorageUploadResult? uploadedImageResult = null;
        if (request.Image is not null)
        {
            var uploadResult =
                await objectStorageService.UploadAsync(file: request.Image, cancellationToken: cancellationToken);
            if (uploadResult.IsSuccess is false)
            {
                logger.LogCritical(message: "Image could not be uploaded. Error: {0}", uploadResult.Message);

                throw new InvalidOperationException(message: Messages.ProductImageUploadFailed);
            }

            uploadedImageResult = uploadResult.Data;
        }

        ISendEndpoint sendEndpoint =
            await sendEndpointProvider.GetSendEndpoint(address: QueueInformations.ProductUpdateRequestQueueUri);

        await sendEndpoint.Send(message: new ProductUpdateMessage(Id: request.Id,
                Name: request.Name,
                Description: request.Description,
                NewImage: uploadedImageResult,
                Price: request.Price),
            cancellationToken: cancellationToken);
        
        logger.LogInformation(message: "Product update request sent successfully to queue. Id: {0}", maskedProductId);
    }
}