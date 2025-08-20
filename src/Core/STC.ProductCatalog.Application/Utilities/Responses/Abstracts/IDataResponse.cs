using STC.ProductCatalog.Domain._Shared.Paginations.Abstracts;

namespace STC.ProductCatalog.Application.Utilities.Responses.Abstracts;

public interface IDataResponse<T> : IResponse
{
    public T? Data { get; set; }

    public IPaginationOut? Pagination { get; set; }
}