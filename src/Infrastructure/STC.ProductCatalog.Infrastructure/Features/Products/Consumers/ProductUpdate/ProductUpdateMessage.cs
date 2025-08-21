namespace STC.ProductCatalog.Infrastructure.Features.Products.Consumers.ProductUpdate;

public record ProductUpdateMessage(string Id, string Name, string Description, string? NewImageUrl, long Price);