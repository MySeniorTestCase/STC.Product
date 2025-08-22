using Microsoft.Extensions.Logging;
using STC.ProductCatalog.Application.Utilities.ObjectStorage;
using STC.ProductCatalog.Domain.Aggregates.Products;
using STC.ProductCatalog.Domain.Aggregates.Products.Repositories;

namespace STC.ProductCatalog.Application.Features.Products.Commands.DeleteProduct;

public class DeleteProductCommandRequestHandler(
    IProductReadRepository productReadRepository,
    IObjectStorageService objectStorageService,
    IProductWriteRepository productWriteRepository,
    ILogger<DeleteProductCommandRequestHandler> logger)
    : IRequestHandler<DeleteProductCommandRequest, IDataResponse<DeleteProductCommandResponse>>
{
    public async Task<IDataResponse<DeleteProductCommandResponse>> Handle(DeleteProductCommandRequest request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Product delete request received.");

        Product? product = await productReadRepository.GetAsync(exp: _product => _product.Id == request.Id,
            cancellationToken: cancellationToken);
        if (product is null)
        {
            logger.LogWarning("Product is not found for deletion.");
            return ResponseCreator.Error<DeleteProductCommandResponse>(message: Messages.ProductIsNotFound);
        }

        await productWriteRepository.HardDeleteAsync(entity: product, cancellationToken: cancellationToken);

        if (await productWriteRepository.SaveChangesAsync(cancellationToken: cancellationToken) is false)
        {
            logger.LogCritical(message: "Failed to delete product from database.");
            throw new InvalidOperationException(message: Messages.ProductCouldNotBeDeleted);
        }

        string[] fileNamesToDelete = product.Medias.Select(_media => _media.FileName).ToArray();
        if (fileNamesToDelete.Length > 0)
        {
            IResponse deleteResult = await objectStorageService.DeleteAsync(fileNames: fileNamesToDelete,
                cancellationToken: cancellationToken);
            if (deleteResult.IsSuccess is false)
                logger.LogError(message: "Failed to delete product media files from object storage.");
        }

        logger.LogInformation("Product deleted successfully.");

        return ResponseCreator.Success(message: Messages.ProductDeletedSuccessfully,
            data: new DeleteProductCommandResponse(Id: product.Id));
    }
}