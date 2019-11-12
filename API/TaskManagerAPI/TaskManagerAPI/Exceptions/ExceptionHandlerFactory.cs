using System;
using TaskManagerAPI.CQRS.Exceptions;
using TaskManagerAPI.Exceptions.Handlers;
using TaskManagerAPI.Repositories.Exceptions;

namespace TaskManagerAPI.Exceptions
{
    public class ExceptionHandlerFactory : IExceptionHandlerFactory
    {
        private readonly 
        public IExceptionHandler GetExceptionHandler(Exception ex)
        {
            if (ex is ServiceException)
            {
                return new DefaultExceptionHandler(ex);
            }
            else if (ex is RepositoryException)
            {
                return new DefaultExceptionHandler(ex);
            }
            else
            {
                return new DefaultExceptionHandler();
            }
        }
    }
}
