using STC.ProductCatalog.Domain._Shared.Events;
using STC.ProductCatalog.Domain._Shared.Types.Abstracts;

namespace STC.ProductCatalog.Domain._Shared.Types.Concretes;

public abstract class EntityBase<TKey> : DomainEventWrapper, IEntity<TKey> where TKey : struct
{
    public TKey Id { get; protected set; }
}