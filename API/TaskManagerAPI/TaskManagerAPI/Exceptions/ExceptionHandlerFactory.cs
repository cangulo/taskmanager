using System;
using TaskManagerAPI.CQRS.Exceptions;
using TaskManagerAPI.Exceptions.Handlers;
using TaskManagerAPI.Models.Errors;

namespace TaskManagerAPI.Exceptions
{
    public class ExceptionHandlerFactory : IExceptionHandlerFactory
    {
        private readonly IErrorToHttpStatusCodeHelper _errorCodeMapper;

        public ExceptionHandlerFactory(IErrorToHttpStatusCodeHelper errorCodeMapper)
        {
            _errorCodeMapper = errorCodeMapper ?? throw new ArgumentNullException(nameof(errorCodeMapper));
        }

        public IExceptionHandler GetExceptionHandler(Exception ex)
        {
            if (ex is ServiceException)
            {
                return new ServiceExceptionHandler(_errorCodeMapper, (ServiceException)ex);
            }
            else
            {
                return new DefaultExceptionHandler();
            }

        }
    }
}
