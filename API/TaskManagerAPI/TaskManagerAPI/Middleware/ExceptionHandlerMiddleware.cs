using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using TaskManagerAPI.Exceptions;
using TaskManagerAPI.Exceptions.Handlers;

namespace TaskManagerAPI.Pipeline
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IExceptionHandlerFactory _exceptionHandlerFactory;
        private readonly ILogger _logger;

        public ExceptionHandlerMiddleware(IExceptionHandlerFactory exceptionHandlerFactory, ILogger logger, RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger;
            _exceptionHandlerFactory = exceptionHandlerFactory ?? throw new ArgumentNullException(nameof(exceptionHandlerFactory));
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.ToString());
                IExceptionHandler exceptionHandler = _exceptionHandlerFactory.GetExceptionHandler(ex);
                context.Response.StatusCode = exceptionHandler.GetHttpStatusCode();
                string responseContent = exceptionHandler.CreateResponseContent();
                await context.Response.WriteAsync(responseContent);
            }
        }
    }
}