namespace STC.ProductCatalog.Application.Features.Products.Queries.GetProducts;

public record GetProductsQueryResponse
{
    private GetProductsQueryResponse()
    {
    }

    public GetProductsQueryResponse(string id)
    {
        Id = id;
    }

    public string Id { get; init; } = null!;
}