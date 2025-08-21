using STC.ProductCatalog.Domain._Shared.Medias;

namespace STC.ProductCatalog.Domain.Aggregates.Products.Services;

public interface IProductDomainService
{
    ValueTask<Product> CreateAsync(string name, string description, Media media, long price,
        CancellationToken cancellationToken);

    ValueTask SetNameAsync(Product product, string name, CancellationToken cancellationToken);
}