using STC.ProductCatalog.Domain._Shared.Events;

namespace STC.ProductCatalog.Application.Utilities.DomainEventDispatcher;

public interface IDomainEventDispatcher
{
    public Task DispatchAsync(DomainEventWrapper @object, CancellationToken cancellationToken);
    public Task DispatchAsync(IEnumerable<DomainEventWrapper> objects, CancellationToken cancellationToken);
}