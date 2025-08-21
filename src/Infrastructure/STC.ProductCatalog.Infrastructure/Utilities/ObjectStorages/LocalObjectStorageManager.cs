using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using STC.ProductCatalog.Application.Utilities.Logging;
using STC.ProductCatalog.Application.Utilities.ObjectStorage;
using STC.ProductCatalog.Application.Utilities.ObjectStorage.Models;
using STC.ProductCatalog.Domain._Shared.Medias;

namespace STC.ProductCatalog.Infrastructure.Utilities.ObjectStorages;

public class LocalObjectStorageManager(IHostEnvironment hostEnvironment, ILogger<LocalObjectStorageManager> logger)
    : IObjectStorageService
{
    private static MediaProvider Provider => MediaProvider.Local;
    private string GetUploadsDirectory() => Path.Combine(hostEnvironment.ContentRootPath, "wwwroot");

    public async ValueTask<IDataResponse<ObjectStorageUploadResult>> UploadAsync(IFormFile file,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(message: "Local file upload starting. File name: {FileName}, Content Type: {ContentType}",
            file.FileName.ToMaskedString(takeHalf: true), file.ContentType);

        string newFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

        string filePath = Path.Combine(GetUploadsDirectory(), newFileName);

        await using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream, cancellationToken: cancellationToken);

        logger.LogInformation(message: "Local file upload done.");

        return ResponseCreator.Success(message: Messages.FileUploadedSuccessfully,
            data: new ObjectStorageUploadResult(Provider: Provider, FileName: newFileName));
    }

    public async ValueTask<IResponse> DeleteAsync(CancellationToken cancellationToken, params string[] fileNames)
    {
        logger.LogInformation(message: "Local file deletion starting. File count: {FileCount}", fileNames.Length);

        await Parallel.ForEachAsync(fileNames, cancellationToken: cancellationToken, body: async (fileName, _) =>
        {
            string path = Path.Combine(Path.Combine(GetUploadsDirectory(), fileName));

            bool isFileNotFound = File.Exists(path: path) is false;
            if (isFileNotFound)
                logger.LogWarning(message: "File not found for deletion. File path: {FilePath}", path);
            else
            {
                File.Delete(path: fileName);

                logger.LogInformation(message: "Local file deletion done.");
            }
        });

        return await ValueTask.FromResult(ResponseCreator.Success(message: Messages.FilesDeletedSuccessfully));
    }

    public string? MergeUrl(Media? media)
    {
        if (media is null || media.Provider != Provider)
            return null;

        return Path.Combine(GetUploadsDirectory(), media.FileName);
    }
}