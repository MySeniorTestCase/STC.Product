using MongoDB.Bson;
using STC.ProductCatalog.Domain._Shared.Events;
using STC.ProductCatalog.Domain._Shared.Types.Abstracts;

namespace STC.ProductCatalog.Domain._Shared.Types.Concretes;

public abstract class AggregateRootBase : DomainEventWrapper, IAggregateRoot<string>
{
    public string Id { get; protected set; } = null!;
    public long Version { get; set; }

    public void GenerateIdAndSetVersion()
    {
        Id = ObjectId.GenerateNewId().ToString();
        Version = 1;
    }
}