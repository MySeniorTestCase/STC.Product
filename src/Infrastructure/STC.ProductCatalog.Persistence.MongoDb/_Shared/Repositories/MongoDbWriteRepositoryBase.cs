using MongoDB.Driver;
using STC.ProductCatalog.Domain._Shared.Repositories;
using STC.ProductCatalog.Domain._Shared.Types.Abstracts;
using STC.ProductCatalog.Persistence.MongoDb._Shared.UnitOfWorks;

namespace STC.ProductCatalog.Persistence.MongoDb._Shared.Repositories;

public class MongoDbWriteRepositoryBase<TDocument, TKey> : IWriteRepository<TDocument, TKey>
    where TDocument : ITable<TKey>
{
    protected readonly IUnitOfWork UnitOfWork;
    protected readonly IMongoCollection<TDocument> Collection;
    protected readonly IClientSessionHandle ClientSessionHandle;

    protected MongoDbWriteRepositoryBase(IMongoCollection<TDocument> collection,
        IClientSessionHandle clientSessionHandle, IUnitOfWork unitOfWork)
    {
        Collection = collection;
        ClientSessionHandle = clientSessionHandle;
        UnitOfWork = unitOfWork;
    }

    public virtual async Task<TDocument> CreateAsync(TDocument entity, CancellationToken cancellationToken)
    {
        UnitOfWork.AddOperation(operation: () => Collection.InsertOneAsync(session: ClientSessionHandle,
            document: entity,
            cancellationToken: cancellationToken)
        );

        return await Task.FromResult(entity);
    }

    public virtual Task CreateBulkAsync(IEnumerable<TDocument> entities, CancellationToken cancellationToken)
    {
        UnitOfWork.AddOperation(operation: () => Collection.InsertManyAsync(
            documents: entities,
            session: ClientSessionHandle,
            cancellationToken: cancellationToken)
        );

        return Task.CompletedTask;
    }

    public Task DirectCreateBulkAsync(IEnumerable<TDocument> entities, CancellationToken cancellationToken)
    {
        return Collection.InsertManyAsync(documents: entities, cancellationToken: cancellationToken);
    }

    public Task UpdateAsync(TDocument entity, CancellationToken cancellationToken)
    {
        var filter = Builders<TDocument>.Filter.Eq(field: field => field.Id, entity.Id);

        UnitOfWork.AddOperation(operation: () => Collection.FindOneAndReplaceAsync(filter: filter,
            replacement: entity,
            session: ClientSessionHandle,
            cancellationToken: cancellationToken)
        );

        return Task.CompletedTask;
    }

    public Task UpdateBulkAsync(IEnumerable<TDocument> entities, CancellationToken cancellationToken)
    {
        List<Task> tasks = new();

        foreach (TDocument entity in entities)
        {
            Task task = UpdateAsync(entity: entity, cancellationToken: cancellationToken);
            tasks.Add(task);
        }

        return Task.WhenAll(tasks);
    }

    public virtual Task HardDeleteAsync(TKey id, CancellationToken cancellationToken)
    {
        var filter = Builders<TDocument>.Filter.Eq(field: field => field.Id, value: id);

        UnitOfWork.AddOperation(operation: () => Collection.DeleteOneAsync(filter: filter,
            session: ClientSessionHandle,
            cancellationToken: cancellationToken)
        );

        return Task.CompletedTask;
    }

    public Task HardDeleteAsync(TDocument entity, CancellationToken cancellationToken)
    {
        var filter = Builders<TDocument>.Filter.Eq(field: field => field.Id, value: entity.Id);

        UnitOfWork.AddOperation(operation: () =>
            Collection.FindOneAndDeleteAsync(filter: filter, cancellationToken: cancellationToken));

        return Task.CompletedTask;
    }

    public virtual async Task<bool> SaveChangesAsync(CancellationToken cancellationToken)
    {
        await UnitOfWork.CommitChanges(cancellationToken: cancellationToken);
        return true;
    }
}