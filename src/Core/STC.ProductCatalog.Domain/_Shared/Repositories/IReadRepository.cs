using System.Linq.Expressions;
using STC.ProductCatalog.Domain._Shared.Paginations.Abstracts;
using STC.ProductCatalog.Domain._Shared.Types.Abstracts;

namespace STC.ProductCatalog.Domain._Shared.Repositories;

public interface IReadRepository<TTable, TKey> : IRepository<TTable, TKey>
    where TTable : ITable<TKey>
{
    Task<bool> AnyAsync(Expression<Func<TTable, bool>> exp, CancellationToken cancellationToken);

    Task<long> CountAsync(CancellationToken cancellationToken, Expression<Func<TTable, bool>>? exp = null);

    Task<TTable?> GetAsync(Expression<Func<TTable, bool>> exp, CancellationToken cancellationToken);

    Task<ICollection<TTable>> GetAllAsync(CancellationToken cancellationToken,
        Expression<Func<TTable, bool>>? exp = null);

    Task<(ICollection<TTable> Data, IPaginationOut Pagination)> GetAllAsync(
        IPaginationArguments pagination,
        CancellationToken cancellationToken,
        Expression<Func<TTable, bool>>? exp = null);
}