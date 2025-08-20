using STC.ProductCatalog.Domain._Shared.Paginations.Abstracts;

namespace STC.ProductCatalog.Domain._Shared.Paginations.Concretes;

public record PaginationArguments(long Page, long? ItemCountInThePage, ISortDefinition[] Sorts) : IPaginationArguments;