using MongoDB.Driver;
using STC.ProductCatalog.Domain.Aggregates.Products;

namespace STC.ProductCatalog.Persistence.MongoDb.Contexts;

public interface IMongoDbContext
{
    public IMongoCollection<Product> Products { get; }
}