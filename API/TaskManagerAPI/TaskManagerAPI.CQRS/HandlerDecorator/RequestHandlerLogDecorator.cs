using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace TaskManagerAPI.CQRS.HandlerDecorator
{
    public class RequestHandlerLogDecorator<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IRequestHandler<TRequest, TResponse> _decorated;
        private readonly ILogger<RequestHandlerLogDecorator<TRequest, TResponse>> _logger;

        public RequestHandlerLogDecorator(IRequestHandler<TRequest, TResponse> decorated, ILogger<RequestHandlerLogDecorator<TRequest, TResponse>> logger)
        {
            _decorated = decorated;
            _logger = logger;
        }

        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
        {
            return _decorated.Handle(request, cancellationToken);
        }
    }
}
