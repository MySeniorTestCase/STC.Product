using STC.ProductCatalog.Domain._Shared.Events;

namespace STC.ProductCatalog.Domain.Aggregates.Products.Events;

public record ProductCreatedDomainEvent(string Id) : IDomainEvent;