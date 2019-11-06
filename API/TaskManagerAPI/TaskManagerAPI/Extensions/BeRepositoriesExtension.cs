using Microsoft.Extensions.DependencyInjection;
using TaskManagerAPI.Helpers;
using TaskManagerAPI.Models.Errors;
using TaskManagerAPI.Repositories.AccountRepository;
using TaskManagerAPI.Repositories.TaskRepository;

namespace TaskManagerAPI.StartupConfiguration.Extensions
{
    /// <summary>
    /// Extended method of IServiceCollection to configure the BE repositories of the Repository Layer. This aims to be a simple and minimum configuration. 
    /// </summary>
    public static class BeRepositoriesExtension
    {
        public static IServiceCollection AddBeRepositories(this IServiceCollection serviceCollection)
        {
            #region repositories
            serviceCollection.AddScoped<IAccountRepository, AccountRepository>();
            serviceCollection.AddScoped<ITasksByAccountRepository, TasksByAccountRepository>();
            #endregion

            #region Errors helpers
            serviceCollection.AddTransient<IErrorToHttpStatusCodeHelper, ErrorToHttpStatusCodeHelper>();
            serviceCollection.AddTransient<IErrorResponseCreator, ErrorResponseCreator>();
            #endregion

            return serviceCollection;
        }
    }
}
