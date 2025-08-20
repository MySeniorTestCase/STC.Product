using Microsoft.AspNetCore.Http;

namespace STC.ProductCatalog.Application.Utilities.ObjectStorage;

public interface IObjectStorageService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="file"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>File access url</returns>
    ValueTask<IDataResponse<string>> UploadAsync(IFormFile file, CancellationToken cancellationToken);

    ValueTask<IResponse> DeleteAsync(string url, CancellationToken cancellationToken);
}