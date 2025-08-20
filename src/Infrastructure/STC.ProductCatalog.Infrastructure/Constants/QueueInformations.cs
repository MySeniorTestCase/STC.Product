namespace STC.ProductCatalog.Infrastructure.Constants;

public static class QueueInformations
{
    public static Uri ProductCreationRequestQueueUri => new Uri("queue:product-creation-request");
}