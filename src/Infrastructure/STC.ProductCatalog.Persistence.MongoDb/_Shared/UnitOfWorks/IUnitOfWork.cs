namespace STC.ProductCatalog.Persistence.MongoDb._Shared.UnitOfWorks;

public interface IUnitOfWork
{
    IDisposable DisposableSession { get; }

    void AddOperation(Func<Task> operation);

    void CleanOperations();

    Task CommitChanges(CancellationToken cancellationToken);
}