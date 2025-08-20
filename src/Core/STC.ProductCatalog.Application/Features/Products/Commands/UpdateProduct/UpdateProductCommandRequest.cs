using Microsoft.AspNetCore.Http;

namespace STC.ProductCatalog.Application.Features.Products.Commands.UpdateProduct;

public record UpdateProductCommandRequest(string Id, string Name, string Description, IFormFile? Image, long Price)
    : IRequest<IResponse>;