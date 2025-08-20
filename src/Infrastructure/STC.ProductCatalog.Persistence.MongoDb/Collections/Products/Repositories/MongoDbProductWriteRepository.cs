using MongoDB.Driver;
using STC.ProductCatalog.Domain.Aggregates.Products;
using STC.ProductCatalog.Domain.Aggregates.Products.Repositories;
using STC.ProductCatalog.Persistence.MongoDb._Shared.Repositories;
using STC.ProductCatalog.Persistence.MongoDb._Shared.UnitOfWorks;
using STC.ProductCatalog.Persistence.MongoDb.Contexts;

namespace STC.ProductCatalog.Persistence.MongoDb.Collections.Products.Repositories;

public class MongoDbProductWriteRepository(
    IMongoDbContext context,
    IClientSessionHandle clientSessionHandle,
    IUnitOfWork unitOfWork)
    : MongoDbWriteRepositoryBase<Product, string>(collection: context.Products,
        clientSessionHandle: clientSessionHandle,
        unitOfWork: unitOfWork), IProductWriteRepository;