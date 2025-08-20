using System.Net;
using STC.ProductCatalog.Application.Utilities.Responses.Abstracts;
using STC.ProductCatalog.Application.Utilities.Responses.Concretes;
using STC.ProductCatalog.Domain._Shared.Paginations.Abstracts;

namespace STC.ProductCatalog.Application.Utilities.Responses;

public static class ResponseCreator
{
    public static IResponse Success(string message,
        HttpStatusCode statusCode = HttpStatusCode.OK,
        ResponseMessageTypes messageType = ResponseMessageTypes.Success)
        => new ResponseBase(message: message, statusCode: statusCode, messageType: messageType, isSuccess: true,
            validationExceptions: null);

    public static IResponse Error(string message,
        HttpStatusCode statusCode = HttpStatusCode.BadRequest,
        ResponseMessageTypes messageType = ResponseMessageTypes.Error,
        List<KeyValuePair<string, string>>? validationExceptions = default)
        => new ResponseBase(message: message, messageType: messageType, statusCode: statusCode, isSuccess: false,
            validationExceptions: validationExceptions);

    public static IResponse Error(IResponse response)
        => new ResponseBase(message: response.Message, messageType: response.MessageType,
            statusCode: response.StatusCode, isSuccess: false,
            validationExceptions: null);

    private static IDataResponse<T> Merge<T>(IResponse response, T? data)
        => new DataResponseBase<T>(response: response, data: data);

    public static IDataResponse<T> Success<T>(string message,
        T data,
        IPaginationOut? pagination = null,
        HttpStatusCode statusCode = HttpStatusCode.OK,
        ResponseMessageTypes messageType = ResponseMessageTypes.Success)
    {
        var result = Merge(response: Success(message: message, messageType: messageType, statusCode: statusCode),
            data: data);
        result.Pagination = pagination;

        return result;
    }

    public static IDataResponse<T> Error<T>(string message, T? data = default,
        IPaginationOut? pagination = null,
        HttpStatusCode statusCode = HttpStatusCode.BadRequest,
        ResponseMessageTypes messageType = ResponseMessageTypes.Error)
    {
        var result = Merge(response: Error(message: message, messageType: messageType, statusCode: statusCode),
            data: data);
        result.Pagination = pagination;
        return result;
    }

    public static IDataResponse<T> Error<T>(IResponse response)
        => Merge(response: response, data: default(T));

    public static DataResponseBase<T> Convert<T>(IDataResponse<T> response)
        => (DataResponseBase<T>)response;
}