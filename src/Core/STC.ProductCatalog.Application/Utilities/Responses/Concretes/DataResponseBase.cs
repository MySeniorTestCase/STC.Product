using System.Net;
using STC.ProductCatalog.Application.Utilities.Responses.Abstracts;
using STC.ProductCatalog.Domain._Shared.Paginations.Abstracts;

namespace STC.ProductCatalog.Application.Utilities.Responses.Concretes;

public sealed class DataResponseBase<T> : ResponseBase, IDataResponse<T>
{
    public DataResponseBase()
    {
    }

    public DataResponseBase(string message, HttpStatusCode statusCode, ResponseMessageTypes messageType, bool isSuccess,
        T? data, IPaginationOut? pagination = null) : base(message, statusCode, messageType: messageType,
        isSuccess, validationExceptions: null)
    {
        Data = data;
        Pagination = pagination;
    }

    public DataResponseBase(IResponse response, T? data, IPaginationOut? pagination = null) : base(
        message: response.Message, statusCode: response.StatusCode,
        messageType: response.MessageType,
        isSuccess: response.IsSuccess, validationExceptions: null)
    {
        Data = data;
        Pagination = pagination;
    }

    public T? Data { get; set; } = default;
    public IPaginationOut? Pagination { get; set; }
}