namespace STC.ProductCatalog.Infrastructure.Features.Products.QueueMessages;

public record ProductCreationMessage(string Name, string Description, string ImageUrl, long Price);