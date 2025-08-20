using STC.ProductCatalog.Domain._Shared.Types.Abstracts;

namespace STC.ProductCatalog.Domain._Shared.Repositories;

public interface IWriteRepository<TTable, TKey> : IRepository<TTable, TKey>
    where TTable : ITable<TKey>
{
    Task<TTable> CreateAsync(TTable entity, CancellationToken cancellationToken);
    Task CreateBulkAsync(IEnumerable<TTable> entities, CancellationToken cancellationToken);
    Task DirectCreateBulkAsync(IEnumerable<TTable> entities, CancellationToken cancellationToken);

    Task UpdateAsync(TTable entity, CancellationToken cancellationToken);
    Task UpdateBulkAsync(IEnumerable<TTable> entities, CancellationToken cancellationToken);

    Task HardDeleteAsync(TKey id, CancellationToken cancellationToken);
    Task HardDeleteAsync(TTable entity, CancellationToken cancellationToken);

    Task<bool> SaveChangesAsync(CancellationToken cancellationToken);
}