namespace STC.ProductCatalog.Application.Features.Products.Commands.CreateProduct;

public record CreateProductCommandRequest(string Name, string Description, string ImageUrl, long Price)
    : IRequest<IDataResponse<CreateProductCommandResponse>>;