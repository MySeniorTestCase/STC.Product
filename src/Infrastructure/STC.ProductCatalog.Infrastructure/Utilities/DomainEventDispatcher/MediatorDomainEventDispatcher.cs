using STC.ProductCatalog.Application.Utilities.DomainEventDispatcher;
using STC.ProductCatalog.Domain._Shared.Events;

namespace STC.ProductCatalog.Infrastructure.Utilities.DomainEventDispatcher;

public class MediatorDomainEventDispatcher(IMediator mediator) : IDomainEventDispatcher
{
    public async Task DispatchAsync(DomainEventWrapper @object, CancellationToken cancellationToken)
    {
        while (@object.GetDomainEvents().Count > 0)
        {
            IDomainEvent domainEvent = @object.GetDomainEvents().Dequeue();
            await mediator.Publish(domainEvent, cancellationToken: cancellationToken);
        }
    }

    public async Task DispatchAsync(IEnumerable<DomainEventWrapper> objects, CancellationToken cancellationToken)
    {
        foreach (DomainEventWrapper domainEvent in objects.Select(_domainEventBase => _domainEventBase))
            await this.DispatchAsync(@object: domainEvent, cancellationToken: cancellationToken);
    }
}