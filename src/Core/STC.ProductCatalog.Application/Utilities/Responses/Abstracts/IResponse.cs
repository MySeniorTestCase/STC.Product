using System.Net;
using STC.ProductCatalog.Application.Utilities.Responses.Concretes;

namespace STC.ProductCatalog.Application.Utilities.Responses.Abstracts;

public interface IResponse
{
    public string Message { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public ResponseMessageTypes MessageType { get; set; }
    public bool IsSuccess { get; set; }
    public List<KeyValuePair<string, string>> ValidationExceptions { get; set; }
}