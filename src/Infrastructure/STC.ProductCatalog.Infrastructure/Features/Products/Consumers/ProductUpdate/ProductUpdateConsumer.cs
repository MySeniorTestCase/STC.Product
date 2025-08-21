using MassTransit;
using STC.ProductCatalog.Application.Features.Products.Commands.UpdateProduct;

namespace STC.ProductCatalog.Infrastructure.Features.Products.Consumers.ProductUpdate;

public class ProductUpdateConsumer(IMediator mediator) : IConsumer<ProductUpdateMessage>
{
    public async Task Consume(ConsumeContext<ProductUpdateMessage> context)
    {
        ProductUpdateMessage message = context.Message;

        UpdateProductCommandRequest request = new(Id: message.Id,
            Name: message.Name,
            Description: message.Description,
            NewImageUrl: message.NewImageUrl,
            Price: message.Price);

        var updateResult = await mediator.Send(request, cancellationToken: context.CancellationToken);
        if (updateResult.IsSuccess is false)
            throw new Exception(message: updateResult.Message, innerException: null);
    }
}