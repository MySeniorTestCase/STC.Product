using System.Net;
using System.Text.Json.Serialization;
using STC.ProductCatalog.Application.Utilities.Responses.Concretes;

namespace STC.ProductCatalog.Application.Utilities.Responses.Abstracts;

[JsonDerivedType(typeof(ResponseBase))]
[JsonDerivedType(typeof(DataResponseBase<>))]
public interface IResponse
{
    public string Message { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public ResponseMessageTypes MessageType { get; set; }
    public bool IsSuccess { get; set; }
    public List<KeyValuePair<string, string>> ValidationExceptions { get; set; }
}