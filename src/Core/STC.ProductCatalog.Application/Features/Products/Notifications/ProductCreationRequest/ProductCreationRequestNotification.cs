using Microsoft.AspNetCore.Http;

namespace STC.ProductCatalog.Application.Features.Products.Notifications.ProductCreationRequest;

public record ProductCreationRequestNotificationRequest(string Name, string Description, IFormFile Image, long Price)
    : INotification;