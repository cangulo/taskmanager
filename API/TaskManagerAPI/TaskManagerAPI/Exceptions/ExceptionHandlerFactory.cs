using System;
using TaskManagerAPI.CQRS.Exceptions;
using TaskManagerAPI.Exceptions.Handlers;

namespace TaskManagerAPI.Exceptions
{
    public class ExceptionHandlerFactory : IExceptionHandlerFactory
    {
        public IExceptionHandler GetExceptionHandler(Exception ex)
        {
            if (ex is CQException)
            {
                return new CQExceptionHandler((CQException)ex);
            }
            else
            {
                return new DefaultExceptionHandler();
            }
        }
    }
}