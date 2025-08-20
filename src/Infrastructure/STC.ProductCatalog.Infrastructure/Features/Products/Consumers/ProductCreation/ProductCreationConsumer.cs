using MassTransit;
using STC.ProductCatalog.Application.Features.Products.Commands.CreateProduct;
using STC.ProductCatalog.Infrastructure.Features.Products.QueueMessages;

namespace STC.ProductCatalog.Infrastructure.Features.Products.Consumers.ProductCreation;

public class ProductCreationConsumer(IMediator mediator) : IConsumer<ProductCreationMessage>
{
    public async Task Consume(ConsumeContext<ProductCreationMessage> context)
    {
        ProductCreationMessage message = context.Message;

        CreateProductCommandRequest request = new(Name: message.Name,
            Description: message.Description,
            ImageUrl: message.ImageUrl,
            Price: message.Price);

        var createResult = await mediator.Send(request, cancellationToken: context.CancellationToken);
        if (createResult.IsSuccess is false)
            throw new Exception(message: createResult.Message, innerException: null);
    }
}