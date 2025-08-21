using MassTransit;
using Microsoft.Extensions.Logging;
using STC.ProductCatalog.Application.Features.Products.Commands.UpdateProduct;
using STC.ProductCatalog.Application.Utilities.Logging;

namespace STC.ProductCatalog.Infrastructure.Features.Products.Consumers.ProductUpdate;

public class ProductUpdateConsumer(IMediator mediator, ILogger<ProductUpdateConsumer> logger)
    : IConsumer<ProductUpdateMessage>
{
    public async Task Consume(ConsumeContext<ProductUpdateMessage> context)
    {
        string maskedProductId = context.Message.Id.ToMaskedString(takeHalf: true);

        logger.LogInformation("{Consumer} received a Product Update message. Product ID: {ProductId}",
            nameof(ProductUpdateConsumer), maskedProductId);

        ProductUpdateMessage message = context.Message;

        UpdateProductCommandRequest request = new(Id: message.Id,
            Name: message.Name,
            Description: message.Description,
            NewImage: message.NewImage,
            Price: message.Price);

        var updateResult = await mediator.Send(request, cancellationToken: context.CancellationToken);
        if (updateResult.IsSuccess is false)
        {
            logger.LogError(message: "{Consumer} consumer failed to update product. Error: {Error}",
                nameof(ProductUpdateConsumer), updateResult.Message);
            throw new Exception(message: updateResult.Message, innerException: null);
        }

        logger.LogInformation("{Consumer} Product Update completed successfully. Product ID: {ProductId}",
            nameof(ProductUpdateConsumer), maskedProductId);
    }
}