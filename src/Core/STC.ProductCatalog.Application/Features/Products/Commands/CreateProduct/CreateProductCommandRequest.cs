using STC.ProductCatalog.Application.Utilities.ObjectStorage.Models;

namespace STC.ProductCatalog.Application.Features.Products.Commands.CreateProduct;

public record CreateProductCommandRequest(string Name, string Description, ObjectStorageUploadResult Image, long Price)
    : IRequest<IDataResponse<CreateProductCommandResponse>>;