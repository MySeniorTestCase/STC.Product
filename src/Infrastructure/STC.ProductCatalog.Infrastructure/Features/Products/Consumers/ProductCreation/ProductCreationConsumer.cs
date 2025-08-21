using MassTransit;
using Microsoft.Extensions.Logging;
using STC.ProductCatalog.Application.Features.Products.Commands.CreateProduct;
using STC.ProductCatalog.Application.Utilities.Logging;

namespace STC.ProductCatalog.Infrastructure.Features.Products.Consumers.ProductCreation;

public class ProductCreationConsumer(IMediator mediator, ILogger<ProductCreationConsumer> logger)
    : IConsumer<ProductCreationMessage>
{
    public async Task Consume(ConsumeContext<ProductCreationMessage> context)
    {
        string maskedName = context.Message.Name.ToMaskedString(takeHalf: true);

        logger.LogInformation("{Consumer} received a new Product Creation message. Product Name: {ProductName}",
            nameof(ProductCreationConsumer), maskedName);

        ProductCreationMessage message = context.Message;

        CreateProductCommandRequest request = new(Name: message.Name,
            Description: message.Description,
            Image: message.Image,
            Price: message.Price);

        var createResult = await mediator.Send(request, cancellationToken: context.CancellationToken);
        if (createResult.IsSuccess is false)
        {
            logger.LogError("{Consumer} consumer failed to create product. Error: {Error}",
                nameof(ProductCreationConsumer), createResult.Message);
            throw new Exception(message: createResult.Message, innerException: null);
        }

        logger.LogInformation("{Consumer} Product Creation completed successfully. Product ID: {ProductId}",
            nameof(ProductCreationConsumer), createResult.Data!.Id.ToMaskedString(takeHalf: true));
    }
}