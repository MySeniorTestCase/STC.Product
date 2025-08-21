using STC.ProductCatalog.Application.Utilities.ObjectStorage.Models;

namespace STC.ProductCatalog.Application.Features.Products.Commands.UpdateProduct;

public record UpdateProductCommandRequest(string Id, string Name, string Description, ObjectStorageUploadResult? NewImage, long Price)
    : IRequest<IResponse>;