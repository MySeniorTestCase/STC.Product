namespace STC.ProductCatalog.Domain._Shared.Events;

public abstract class DomainEventWrapper
{
    private Queue<IDomainEvent> DomainEvents { get; set; } = [];

    protected void AddDomainEvent(IDomainEvent domainEvent, bool checkDistinct = true)
    {
        if (checkDistinct)
        {
            bool isDomainEventAlreadyAdded =
                DomainEvents.Any(_domainEvent => _domainEvent.GetType().Name == domainEvent.GetType().Name);
            if (isDomainEventAlreadyAdded)
                return;
        }

        DomainEvents.Enqueue(domainEvent);
    }

    public Queue<IDomainEvent> GetDomainEvents() => DomainEvents;
    public void ClearDomainEvents() => DomainEvents.Clear();
}