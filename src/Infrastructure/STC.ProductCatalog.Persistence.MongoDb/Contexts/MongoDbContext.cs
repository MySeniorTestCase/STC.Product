using MongoDB.Driver;
using STC.ProductCatalog.Domain.Aggregates.Products;

namespace STC.ProductCatalog.Persistence.MongoDb.Contexts;

public class MongoDbContext(IMongoClient mongoClient, IClientSessionHandle clientSessionHandle) : IMongoDbContext
{
    private IMongoDatabase Database => mongoClient.GetDatabase(name: "product-catalog");

    public IMongoCollection<Product> Products => Database.GetCollection<Product>(name: "products");
}