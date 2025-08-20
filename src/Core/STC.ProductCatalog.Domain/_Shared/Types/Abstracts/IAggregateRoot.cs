namespace STC.ProductCatalog.Domain._Shared.Types.Abstracts;

public interface IAggregateRoot<TKey> : IRecordVersion, ITable<TKey>
{
}