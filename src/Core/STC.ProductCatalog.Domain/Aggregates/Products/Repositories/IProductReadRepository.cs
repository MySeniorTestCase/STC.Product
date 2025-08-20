using STC.ProductCatalog.Domain._Shared.Repositories;

namespace STC.ProductCatalog.Domain.Aggregates.Products.Repositories;

public interface IProductReadRepository : IReadRepository<Product, string>
{
}