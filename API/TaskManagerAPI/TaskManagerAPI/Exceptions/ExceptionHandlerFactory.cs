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
            if (ex is CQException)
            {
                return new CQExceptionHandler(_errorCodeMapper, (CQException)ex);
            }
            else
            {
                return new DefaultExceptionHandler();
            }

        }
    }
}
