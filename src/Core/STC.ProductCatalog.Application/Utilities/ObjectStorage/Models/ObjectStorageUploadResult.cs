using STC.ProductCatalog.Domain._Shared.Medias;

namespace STC.ProductCatalog.Application.Utilities.ObjectStorage.Models;

public record ObjectStorageUploadResult(MediaProvider Provider, string FileName);