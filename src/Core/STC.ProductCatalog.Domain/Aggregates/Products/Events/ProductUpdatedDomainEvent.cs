using STC.ProductCatalog.Domain._Shared.Events;

namespace STC.ProductCatalog.Domain.Aggregates.Products.Events;

public record ProductUpdatedDomainEvent(string Id) : IDomainEvent;