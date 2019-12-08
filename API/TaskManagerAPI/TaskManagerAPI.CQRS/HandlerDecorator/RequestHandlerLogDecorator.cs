using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace TaskManagerAPI.CQRS.HandlerDecorator
{
    public class RequestHandlerLogDecorator<TRequest, TResult> : IRequestHandler<TRequest, TResult> where TRequest : IRequest<TResult>
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
            this._logger.LogInformation($"Handler;{handlerName};{stopwatch.ElapsedMilliseconds}");
            return response;
        }
    }
}
