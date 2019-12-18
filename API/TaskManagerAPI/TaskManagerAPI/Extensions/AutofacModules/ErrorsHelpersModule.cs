using Autofac;
using TaskManagerAPI.Exceptions;
using TaskManagerAPI.Exceptions.Helpers;
using TaskManagerAPI.Models.Errors;

namespace TaskManagerAPI.Extensions.AutofacModules
{
    public class ErrorsHelpersModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<ErrorToHttpStatusCodeHelper>().As<IErrorToHttpStatusCodeHelper>();
            containerBuilder.RegisterType<ErrorResponseCreator>().As<IErrorResponseCreator>();
            containerBuilder.RegisterType<ExceptionHandlerFactory>().As<IExceptionHandlerFactory>();
        }
    }
}