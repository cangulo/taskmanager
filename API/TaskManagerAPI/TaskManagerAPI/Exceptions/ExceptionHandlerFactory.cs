using System;
using TaskManagerAPI.CQRS.Exceptions;
using TaskManagerAPI.Exceptions.Handlers;
using TaskManagerAPI.Models.Errors;
using TaskManagerAPI.Repositories.Exceptions;

namespace TaskManagerAPI.Exceptions
{
    public class ExceptionHandlerFactory : IExceptionHandlerFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ExceptionHandlerFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IExceptionHandler GetExceptionHandler(Exception ex)
        {
            if (ex is ServiceException)
            {
                IErrorToHttpStatusCodeHelper errorHelpers = (IErrorToHttpStatusCodeHelper)_serviceProvider.GetService(typeof(IErrorToHttpStatusCodeHelper));
                if (errorHelpers != null)
                {
                    return new ServiceExceptionHandler(errorHelpers, (ServiceException)ex);
                }
            }
            else if (ex is RepositoryException)
            {
                return new DefaultExceptionHandler();
            }
            return new DefaultExceptionHandler();
        }
    }
}
