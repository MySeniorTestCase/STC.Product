using STC.ProductCatalog.Domain._Shared.Medias;
using STC.ProductCatalog.Domain._Shared.Types.Concretes;
using STC.ProductCatalog.Domain.Aggregates.Products.Events;
using STC.ProductCatalog.Domain.Constants;

namespace STC.ProductCatalog.Domain.Aggregates.Products;

public class Product : AggregateRootBase
{
    private Product()
    {
    }

    private Product(string name, string description, Media media, long price, DateTime createdAt)
    {
        SetName(name: name);
        SetDescription(description: description);
        AddMedia(media: media);
        SetPrice(price: price);
        CreatedAt = createdAt;
    }

    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public ICollection<Media> Medias { get; private set; } = [];
    public long Price { get; private set; }
    public DateTime CreatedAt { get; private set; }

    internal static bool ValidateName(string name, bool throwException = true)
    {
        const int minLength = 3, maxLength = 100;

        if (name.Length is < minLength or > maxLength)
        {
            if (throwException)
                throw new ArgumentOutOfRangeException(
                    message: string.Format(Messages.InvalidProductNameLength, arg0: minLength, arg1: maxLength),
                    paramName: nameof(name));

            return false;
        }

        return true;
    }

    internal void SetName(string name)
    {
        ValidateName(name: name, throwException: true);

        Name = name;

        bool isInitialized = string.IsNullOrEmpty(this.Id);
        if (isInitialized is false)
            this.AddDomainEvent(new ProductUpdatedDomainEvent(Id: this.Id));
    }

    internal static bool ValidateDescription(string description, bool throwException = true)
    {
        const int minLength = 3, maxLength = 2500;

        if (description.Length is < minLength or > maxLength)
        {
            if (throwException)
                throw new ArgumentOutOfRangeException(
                    message: string.Format(Messages.InvalidProductDescriptionLength, arg0: minLength, arg1: maxLength),
                    paramName: nameof(description));

            return false;
        }

        return true;
    }

    public void SetDescription(string description)
    {
        ValidateDescription(description: description, throwException: true);

        Description = description;

        bool isInitialized = string.IsNullOrEmpty(this.Id);
        if (isInitialized is false)
            this.AddDomainEvent(new ProductUpdatedDomainEvent(Id: this.Id));
    }

    public void AddMedia(Media media)
    {
        Medias.Add(media);

        bool isInitialized = string.IsNullOrEmpty(this.Id);
        if (isInitialized is false)
            this.AddDomainEvent(new ProductUpdatedDomainEvent(Id: this.Id));
    }


    private static bool ValidatePrice(long price, bool throwException = true)
    {
        if (price <= 0)
        {
            if (throwException)
                throw new ArgumentOutOfRangeException(message: Messages.PriceMustBeGreaterThanZero,
                    paramName: nameof(price));

            return false;
        }

        return true;
    }

    public void SetPrice(long price)
    {
        ValidatePrice(price: price);

        Price = price;

        bool isInitialized = string.IsNullOrEmpty(this.Id);
        if (isInitialized is false)
            this.AddDomainEvent(new ProductUpdatedDomainEvent(Id: this.Id));
    }

    public static Product Create(string name, string description, Media media, long price)
    {
        Product instance = new Product(name: name,
            description: description,
            media: media,
            price: price,
            createdAt: DateTime.UtcNow);

        instance.GenerateIdAndSetVersion();

        instance.AddDomainEvent(domainEvent: new ProductCreatedDomainEvent(Id: instance.Id));

        return instance;
    }

    public static (bool IsValid, string[] Errors) PreValidate(string name, string description, long price)
    {
        ICollection<string> errors = [];

        try
        {
            ValidateName(name: name);
            ValidateDescription(description: description);
            ValidatePrice(price: price);
        }
        catch (Exception e)
        {
            errors.Add(e.Message);
        }

        return (IsValid: errors.Count is 0, Errors: errors.ToArray());
    }
}