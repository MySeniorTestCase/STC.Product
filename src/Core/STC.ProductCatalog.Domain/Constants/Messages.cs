namespace STC.ProductCatalog.Domain.Constants;

public static class Messages
{
    public const string InvalidProductNameLength = "Product name must be between {0} and {1} characters.";
    public const string InvalidProductDescriptionLength = "Product description must be between {0} and {1} characters.";
    public const string PriceMustBeGreaterThanZero = "Price must be greater than zero.";
    public const string TheProductNameAlreadyExists = "The product name already exists.";
    public const string ProductMustHaveAtLeastOneImage = "Product must have at least one image.";
    public const string ProductCouldNotBeCreated = "Product could not be created.";
    public const string ProductCreatedSuccessfully = "Product created successfully.";
    public const string YourProductCreationRequestHasBeenQueued = "Your product creation request has been queued.";
    public const string ImageUrlCannotBeEmpty = "Image url can not be empty.";
    public const string ProductIsNotFound = "Product is not found.";
    public const string ProductCouldNotBeUpdated = "Product could not be updated.";
    public const string ProductUpdatedSuccessfully = "Product updated successfully.";
    public const string ProductImageUploadFailed = "Product image upload failed.";
}