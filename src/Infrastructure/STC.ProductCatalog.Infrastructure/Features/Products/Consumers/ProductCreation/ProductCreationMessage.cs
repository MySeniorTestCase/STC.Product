using STC.ProductCatalog.Application.Utilities.ObjectStorage.Models;
using STC.ProductCatalog.Domain._Shared.Medias;

namespace STC.ProductCatalog.Infrastructure.Features.Products.Consumers.ProductCreation;

public record ProductCreationMessage(string Name, string Description, ObjectStorageUploadResult Image, long Price);