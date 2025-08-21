namespace STC.ProductCatalog.Application.Features.Products.Queries.GetProducts;

public record GetProductsQueryResponse
{
    private GetProductsQueryResponse()
    {
    }

    public GetProductsQueryResponse(string id, string name, string description, string imageUrl, decimal price)
    {
        Id = id;
        Name = name;
        Description = description;
        ImageUrl = imageUrl;
        Price = price;
    }

    public string Id { get; init; } = null!;
    public string Name { get; init; } = null!;
    public string Description { get; init; } = null!;
    public string ImageUrl { get; init; } = null!;
    public decimal Price { get; init; }
}