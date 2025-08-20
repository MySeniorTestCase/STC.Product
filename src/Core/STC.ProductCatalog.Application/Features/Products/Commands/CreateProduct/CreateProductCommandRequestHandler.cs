using STC.ProductCatalog.Domain.Aggregates.Products;
using STC.ProductCatalog.Domain.Aggregates.Products.Repositories;
using STC.ProductCatalog.Domain.Aggregates.Products.Services;

namespace STC.ProductCatalog.Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommandRequestHandler(
    IProductDomainService productDomainService,
    IProductWriteRepository productWriteRepository)
    : IRequestHandler<CreateProductCommandRequest, IDataResponse<CreateProductCommandResponse>>
{
    public async Task<IDataResponse<CreateProductCommandResponse>> Handle(CreateProductCommandRequest request,
        CancellationToken cancellationToken)
    {
        Product product = await productDomainService.CreateAsync(name: request.Name,
            description: request.Description,
            imageUrl: request.ImageUrl,
            price: request.Price,
            cancellationToken: cancellationToken);

        await productWriteRepository.CreateAsync(entity: product, cancellationToken: cancellationToken);

        if (await productWriteRepository.SaveChangesAsync(cancellationToken: cancellationToken) is false)
            throw new InvalidOperationException(message: Messages.ProductCouldNotBeCreated);

        return ResponseCreator.Success(message: Messages.ProductCreatedSuccessfully,
            data: new CreateProductCommandResponse(Id: product.Id));
    }
}