using STC.ProductCatalog.Domain._Shared.Paginations.Abstracts;

namespace STC.ProductCatalog.Domain._Shared.Paginations.Concretes;

public record PaginationOut(long Page, long Count, long TotalCount) : IPaginationOut;