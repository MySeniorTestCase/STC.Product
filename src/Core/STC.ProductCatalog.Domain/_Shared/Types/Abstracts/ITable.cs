namespace STC.ProductCatalog.Domain._Shared.Types.Abstracts;

public interface ITable<TKey>
{
    public TKey Id { get; }
}