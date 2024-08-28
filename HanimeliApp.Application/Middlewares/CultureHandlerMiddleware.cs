using System.Globalization;
using Microsoft.AspNetCore.Http;

namespace HanimeliApp.Application.Middlewares
{
	public class CultureHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public CultureHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            var culture = httpContext.Request.RouteValues["culture"]?.ToString();

            if (culture != null)
                CultureInfo.CurrentUICulture = new CultureInfo(culture);

            await _next(httpContext);
        }
    }
}
