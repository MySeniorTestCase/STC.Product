namespace STC.ProductCatalog.Domain._Shared.Types.Abstracts;

public interface IEntity<TKey> : ITable<TKey> where TKey : struct
{
}