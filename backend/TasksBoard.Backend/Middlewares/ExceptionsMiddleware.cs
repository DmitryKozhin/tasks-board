using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace TasksBoard.Backend.Middlewares
{
    public class ExceptionsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionsMiddleware> _logger;

        public ExceptionsMiddleware(RequestDelegate next, ILogger<ExceptionsMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (NotImplementedException exception)
            {
                _logger.LogError(exception, "Вызываемый метод не реализован.");
                await HandleException(httpContext, exception);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Ошибка сервера.");
                await HandleException(httpContext, exception);
            }
        }

        private async Task HandleException(
            HttpContext httpContext,
            Exception exception,
            int exceptionCode = 500)
        {
            var dict = new KeyValuePair<string, StringValues>[httpContext.Response.Headers.Count];
            httpContext.Response.Headers.CopyTo(dict, 0);
            httpContext.Response.Clear();
            foreach (var header in dict)
            {
                httpContext.Response.Headers.Add(header);
            }

            httpContext.Response.StatusCode = exceptionCode;
            await httpContext.Response.WriteAsync(exception.Message);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ExceptionsMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionsMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionsMiddleware>();
        }
    }
}