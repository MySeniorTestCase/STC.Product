using System.Net;
using STC.ProductCatalog.Application.Utilities.Responses;
using STC.ProductCatalog.Application.Utilities.Responses.Abstracts;

namespace STC.ProductCatalog.WebAPI.Middlewares;

public class GlobalExceptionHandlerMiddleware(ILogger<GlobalExceptionHandlerMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context: context);
        }
        catch (Exception e)
        {
            IResponse errorResponse;
            if (e is InvalidOperationException inv)
            {
                logger.LogCritical(
                    message: "An unhandled exception occurred: Message: {ExceptionMessage}, StackTrace: {StackTrace}",
                    e.Message, inv.StackTrace);

                errorResponse = ResponseCreator.Error(message: "An unknown error occured. Please try again later.",
                    statusCode: HttpStatusCode.InternalServerError);
            }
            else
            {
                logger.LogError(message: "An unhandled exception occurred: {ExceptionMessage}", e.Message);

                errorResponse = ResponseCreator.Error(message: e.Message,
                    statusCode: HttpStatusCode.InternalServerError);
            }

            context.Response.StatusCode = (int)errorResponse.StatusCode;
            await context.Response.WriteAsJsonAsync(value:  ResponseCreator.Convert(errorResponse), cancellationToken: context.RequestAborted);
        }
    }
}