using Microsoft.AspNetCore.Http;
using STC.ProductCatalog.Application.Utilities.ObjectStorage.Models;
using STC.ProductCatalog.Domain._Shared.Medias;

namespace STC.ProductCatalog.Application.Utilities.ObjectStorage;

public interface IObjectStorageService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="file"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>File access url</returns>
    ValueTask<IDataResponse<ObjectStorageUploadResult>>UploadAsync(IFormFile file, CancellationToken cancellationToken);

    ValueTask<IResponse> DeleteAsync(CancellationToken cancellationToken, params string[] fileNames);

    string? MergeUrl(Media? media);
}