namespace STC.ProductCatalog.Domain.Aggregates.Products.Services;

public interface IProductDomainService
{
    ValueTask<Product> CreateAsync(string name, string description, string imageUrl, long price,
        CancellationToken cancellationToken);

    ValueTask SetNameAsync(Product product, string name, CancellationToken cancellationToken);
}