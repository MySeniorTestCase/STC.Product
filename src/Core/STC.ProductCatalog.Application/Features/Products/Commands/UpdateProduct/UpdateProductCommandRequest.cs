namespace STC.ProductCatalog.Application.Features.Products.Commands.UpdateProduct;

public record UpdateProductCommandRequest(string Id, string Name, string Description, string? NewImageUrl, long Price)
    : IRequest<IResponse>;