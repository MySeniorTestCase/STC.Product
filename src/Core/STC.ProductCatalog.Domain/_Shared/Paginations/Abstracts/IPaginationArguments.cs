namespace STC.ProductCatalog.Domain._Shared.Paginations.Abstracts;

public interface IPaginationArguments
{
    public long Page { get; init; }
    public long? ItemCountInThePage { get; init; }
    public ISortDefinition[] Sorts { get; init; }
}