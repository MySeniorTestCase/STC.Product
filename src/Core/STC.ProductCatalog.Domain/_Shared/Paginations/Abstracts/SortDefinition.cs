using STC.ProductCatalog.Domain._Shared.Paginations.Concretes;

namespace STC.ProductCatalog.Domain._Shared.Paginations.Abstracts;

public class SortDefinition
{
    public OrderBy OrderBy { get; init; }
    public string Field { get; init; } = null!;
}