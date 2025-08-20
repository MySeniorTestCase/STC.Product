using STC.ProductCatalog.Domain._Shared.Paginations.Abstracts;

namespace STC.ProductCatalog.Application.Utilities.Pagination;

public class PaginationRequest : IPaginationArguments
{
    public long Page { get; init; }
    public long? ItemCountInThePage { get; init; }
    public SortDefinition[] Sorts { get; init; } = [];
}