using STC.ProductCatalog.Domain.Aggregates.Products;
using STC.ProductCatalog.Domain.Aggregates.Products.Repositories;
using STC.ProductCatalog.Persistence.MongoDb._Shared.Repositories;
using STC.ProductCatalog.Persistence.MongoDb.Contexts;

namespace STC.ProductCatalog.Persistence.MongoDb.Collections.Products.Repositories;

public class MongoDbProductReadRepository(IMongoDbContext context)
    : MongoDbReadRepository<Product, string>(collection: context.Products), IProductReadRepository;