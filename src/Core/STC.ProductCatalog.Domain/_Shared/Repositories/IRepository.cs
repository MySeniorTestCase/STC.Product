using STC.ProductCatalog.Domain._Shared.Types.Abstracts;

namespace STC.ProductCatalog.Domain._Shared.Repositories;

public interface IRepository<TTable, TKey> where TTable : ITable<TKey>
{
}