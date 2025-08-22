namespace STC.ProductCatalog.Application.Features.Products.Commands.DeleteProduct;

public record DeleteProductCommandRequest(string Id) : IRequest<IDataResponse<DeleteProductCommandResponse>>;