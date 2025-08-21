using STC.ProductCatalog.Application.Utilities.ObjectStorage.Models;

namespace STC.ProductCatalog.Infrastructure.Features.Products.Consumers.ProductUpdate;

public record ProductUpdateMessage(string Id, string Name, string Description, ObjectStorageUploadResult? NewImage, long Price);