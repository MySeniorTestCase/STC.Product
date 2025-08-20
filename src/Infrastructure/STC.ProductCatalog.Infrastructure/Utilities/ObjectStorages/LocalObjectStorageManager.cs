using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using STC.ProductCatalog.Application.Utilities.ObjectStorage;

namespace STC.ProductCatalog.Infrastructure.Utilities.ObjectStorages;

public class LocalObjectStorageManager(IHostEnvironment hostEnvironment) : IObjectStorageService
{
    private string GetUploadsDirectory()
    {
        return Path.Combine(hostEnvironment.ContentRootPath, "wwwroot");
    }

    public async ValueTask<IDataResponse<string>> UploadAsync(IFormFile file, CancellationToken cancellationToken)
    {
        string filePath = Path.Combine(GetUploadsDirectory(), file.FileName);

        await using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream, cancellationToken: cancellationToken);

        return ResponseCreator.Success(message: Messages.FileUploadedSuccessfully, data: filePath);
    }

    public async ValueTask<IResponse> DeleteAsync(string url, CancellationToken cancellationToken)
    {
        bool isExists = File.Exists(path: url);
        if (isExists is false)
            return ResponseCreator.Success(message: string.Empty);

        File.Delete(path: url);

        return await ValueTask.FromResult(ResponseCreator.Success(message: Messages.FileDeletedSuccessfully));
    }
}