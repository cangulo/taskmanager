using System;
using TaskManagerAPI.Exceptions.Handlers;

namespace TaskManagerAPI.Exceptions
{
    public interface IExceptionHandlerFactory
    {
        IExceptionHandler GetExceptionHandler(Exception ex);
    }
}
