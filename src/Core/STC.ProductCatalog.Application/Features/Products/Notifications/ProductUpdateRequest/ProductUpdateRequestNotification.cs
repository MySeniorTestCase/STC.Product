using Microsoft.AspNetCore.Http;

namespace STC.ProductCatalog.Application.Features.Products.Notifications.ProductUpdateRequest;

public record ProductUpdateRequestNotificationRequest(
    string Id,
    string Name,
    string Description,
    IFormFile? Image,
    long Price)
    : INotification;