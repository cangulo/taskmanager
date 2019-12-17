using Microsoft.Extensions.DependencyInjection;
using TaskManagerAPI.Exceptions;
using TaskManagerAPI.Exceptions.Helpers;
using TaskManagerAPI.Models.Errors;

namespace TaskManagerAPI.StartupConfiguration.Extensions
{
    /// <summary>
    /// Extended method of IServiceCollection to configure the BE Errors classes such as Mappers and creators. This aims to be a simple and minimum configuration. 
    /// </summary>
    /// TODO: Migrate to Autofac
    public static class BeErrorsHelpersExtension
    {
        public static IServiceCollection AddBeErrorsHelpers(this IServiceCollection serviceCollection)
        {

            #region Errors helpers
            serviceCollection.AddTransient<IErrorToHttpStatusCodeHelper, ErrorToHttpStatusCodeHelper>();
            serviceCollection.AddTransient<IErrorResponseCreator, ErrorResponseCreator>();
            #endregion

            #region ExceptionHelpers
            serviceCollection.AddTransient<IExceptionHandlerFactory, ExceptionHandlerFactory>();
            #endregion

            return serviceCollection;
        }
    }
}
