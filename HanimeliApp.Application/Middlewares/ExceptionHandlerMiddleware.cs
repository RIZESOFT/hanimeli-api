using System.Net;
using HanimeliApp.Application.Models;
using HanimeliApp.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace HanimeliApp.Application.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, ILogger<ExceptionHandlerMiddleware> logger)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                ExceptionBase exception = ExceptionBase.FromException(ex);
                var httpStatusCode = GetHttpStatusCodeByExceptionCategory(exception);
                httpContext.Response.StatusCode = (int)httpStatusCode;
                if(httpStatusCode != HttpStatusCode.NoContent)
                    await httpContext.Response.WriteAsJsonAsync(Result.AsFailure(exception));
            }
        }

        private static HttpStatusCode GetHttpStatusCodeByExceptionCategory(ExceptionBase ex) => ex.Category switch
        {
            ExceptionCategories.Authentication => HttpStatusCode.Unauthorized,
            ExceptionCategories.Authorization=> HttpStatusCode.Forbidden,
            ExceptionCategories.Validation or ExceptionCategories.Verification => HttpStatusCode.BadRequest,
            ExceptionCategories.NoContent => HttpStatusCode.NoContent,
            ExceptionCategories.Information => HttpStatusCode.OK,
            _ => HttpStatusCode.InternalServerError,
        };
    }
}
