using MongoDB.Driver;

namespace STC.ProductCatalog.Persistence.MongoDb._Shared.UnitOfWorks;

public class MongoDbUnitOfWork(IClientSessionHandle session) : IUnitOfWork
{
    public IDisposable DisposableSession => session;


    private readonly ICollection<Func<Task>> _operations = new List<Func<Task>>();

    public void AddOperation(Func<Task> operation) => _operations.Add(operation);

    public void CleanOperations() => _operations.Clear();

    public async Task CommitChanges(CancellationToken cancellationToken)
    {
        session.StartTransaction();

        foreach (Func<Task> operation in _operations)
            await operation.Invoke();

        await session.CommitTransactionAsync(cancellationToken: cancellationToken);

        this.CleanOperations();
    }
}