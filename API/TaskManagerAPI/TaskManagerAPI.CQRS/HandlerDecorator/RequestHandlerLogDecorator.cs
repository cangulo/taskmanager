using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskManagerAPI.CQRS.Exceptions;
using TaskManagerAPI.Models.Errors;

namespace TaskManagerAPI.CQRS.HandlerDecorator
{
    public class RequestHandlerLogDecorator<TRequest, TResult> : IRequestHandler<TRequest, TResult> where TRequest : IRequest<TResult> where TResult : FluentResults.ResultBase
    {
        private readonly IRequestHandler<TRequest, TResult> _decorated;
        private readonly ILogger<RequestHandlerLogDecorator<TRequest, TResult>> _logger;

        public RequestHandlerLogDecorator(IRequestHandler<TRequest, TResult> decorated, ILogger<RequestHandlerLogDecorator<TRequest, TResult>> logger)
        {
            _decorated = decorated;
            _logger = logger;
        }

        public async Task<TResult> Handle(TRequest request, CancellationToken cancellationToken)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            TResult response = await _decorated.Handle(request, cancellationToken);
            stopwatch.Stop();
            string handlerName = this._decorated.GetType().Name;
            if ((response as ResultBase).IsFailed)
            {
                // TODO: The responsability of log the errors should be at the top level of the application, at the Exception middleware
                this._logger.LogError($"Handler;{handlerName};{stopwatch.ElapsedMilliseconds}");
                var listError = (response as ResultBase).Errors;
                this._logger.LogError($"Handler;{handlerName};Errors:{string.Join(";", listError.Select(er => er.ToString()))}");
                throw new CQException(listError.Select(error => error as ErrorCodeAndMessage));
            }
            else
            {
                // TODO: We should trace using the Azure App Insights telemetry
                this._logger.LogInformation($"Handler;{handlerName};{stopwatch.ElapsedMilliseconds}");
            }
            return response;
        }
    }
}
