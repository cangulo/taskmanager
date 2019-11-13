using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using TaskManagerAPI.Exceptions;
using TaskManagerAPI.Exceptions.Handlers;
using TaskManagerAPI.Models.Errors;
using TaskManagerAPI.Resources.Errors;

namespace TaskManagerAPI.Pipeline
{
    /// <summary>
    /// Exception Handler that will return the exception information if the environment is not PROD. Otherwise, it will return a generic error message.
    /// </summary>
    public static class PipelineExceptionHandler
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder app, IHostingEnvironment env, ILogger _logger)
        {
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    context.Response.ContentType = "application/json";
                    IExceptionHandlerPathFeature exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                    Exception exception = exceptionHandlerPathFeature.Error;
                    _logger.LogError(exception, "Exception:");

                    if (env.IsProduction() || env.IsStaging())
                    {
                        IExceptionHandlerFactory exceptionHandlerFactory = (IExceptionHandlerFactory)context.RequestServices.GetService(typeof(IExceptionHandlerFactory));
                        if (exceptionHandlerFactory != null)
                        {
                            IExceptionHandler exceptionHandler = exceptionHandlerFactory.GetExceptionHandler(exception);
                            await exceptionHandler.AddErrorResponse(context.Response);
                        }
                        else
                        {
                            DefaultExceptionHandler defaultExceptionHandler = new DefaultExceptionHandler();
                            await defaultExceptionHandler.AddErrorResponse(context.Response);
                        }

                    }
                    else
                    {
                        string result = JsonConvert.SerializeObject(exception);
                        await context.Response.WriteAsync(result);
                    }
                });
            });
            return app;
        }
    }
}
