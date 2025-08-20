using STC.ProductCatalog.Domain._Shared.Repositories;

namespace STC.ProductCatalog.Domain.Aggregates.Products.Repositories;

public interface IProductWriteRepository : IWriteRepository<Product, string>
{
}