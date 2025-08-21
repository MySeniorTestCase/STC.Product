namespace STC.ProductCatalog.Infrastructure.Features.Products.Consumers.ProductCreation;

public record ProductCreationMessage(string Name, string Description, string ImageUrl, long Price);