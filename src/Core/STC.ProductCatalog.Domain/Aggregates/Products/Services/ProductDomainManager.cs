using STC.ProductCatalog.Domain._Shared.Medias;
using STC.ProductCatalog.Domain.Aggregates.Products.Repositories;
using STC.ProductCatalog.Domain.Constants;

namespace STC.ProductCatalog.Domain.Aggregates.Products.Services;

public class ProductDomainManager(IProductReadRepository productReadRepository) : IProductDomainService
{
    public async ValueTask<Product> CreateAsync(string name, string description, Media media, long price,
        CancellationToken cancellationToken)
    {
        bool isNameExists = await productReadRepository.AnyAsync(exp: _product => _product.Name == name,
            cancellationToken: cancellationToken);
        if (isNameExists)
            throw new ArgumentException(message: Messages.TheProductNameAlreadyExists, innerException: null);

        return Product.Create(name: name, description: description, media: media, price: price);
    }

    public async ValueTask SetNameAsync(Product product, string name, CancellationToken cancellationToken)
    {
        if (product.Name == name)
            return;

        Product.ValidateName(name: name);

        bool isNameExists = await productReadRepository.AnyAsync(exp: _product => _product.Name == name,
            cancellationToken: cancellationToken);
        if (isNameExists)
            throw new ArgumentException(message: Messages.TheProductNameAlreadyExists, innerException: null);

        product.SetName(name: name);
    }
}