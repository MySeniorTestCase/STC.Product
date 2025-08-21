namespace STC.ProductCatalog.Infrastructure.Constants;

public static class QueueInformations
{
    public const string ProductCreationRequestQueue = "product-creation-request";
    public static Uri ProductCreationRequestQueueUri => new Uri($"queue:{ProductCreationRequestQueue}");
    
    public const string ProductUpdateRequestQueue = "product-update-request";
    public static Uri ProductUpdateRequestQueueUri => new Uri($"queue:{ProductUpdateRequestQueue}");
}