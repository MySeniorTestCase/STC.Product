using System.Text.Json.Serialization;
using STC.ProductCatalog.Application.Utilities.Responses.Concretes;
using STC.ProductCatalog.Domain._Shared.Paginations.Abstracts;

namespace STC.ProductCatalog.Application.Utilities.Responses.Abstracts;

[JsonDerivedType(typeof(DataResponseBase<>))]
public interface IDataResponse<T> : IResponse
{
    public T? Data { get; set; }

    public IPaginationOut? Pagination { get; set; }
}