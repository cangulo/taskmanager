using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using TaskManagerAPI.Models.Errors;
using TaskManagerAPI.Resources.Errors;

namespace TaskManagerAPI.Pipeline
{
    /// <summary>
    /// Custom Exception Handler that will return the exception information if the environment is not PROD. Otherwise, it will return a generic error message.
    /// </summary>
    public static class CustomExceptionHandler
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder app, IHostingEnvironment env, ILogger _logger)
        {
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = 500;

                    string result = string.Empty;
                    if (!env.IsProduction())
                    {
                        IExceptionHandlerPathFeature exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                        Exception exception = exceptionHandlerPathFeature.Error;
                        _logger.LogError(exception, "Exception throw");
                        result = JsonConvert.SerializeObject(exception);

                    }
                    else
                    {
                        ErrorCodeAndMessage genericError = new ErrorCodeAndMessage(
                                    ErrorsCodesContants.UNKNOWN_ERROR_API, ErrorsMessagesConstants.UNKNOWN_ERROR_API);
                        result = JsonConvert.SerializeObject(genericError);
                        _logger.LogError($"Generic Error throw {result}");
                    }
                    await context.Response.WriteAsync(result);
                });
            });
            return app;
        }
    }
}
