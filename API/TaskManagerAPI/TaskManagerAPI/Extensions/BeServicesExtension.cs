using Microsoft.Extensions.DependencyInjection;
using TaskManagerAPI.BL.AuthProcess;
using TaskManagerAPI.BL.CurrentUserService;
using TaskManagerAPI.BL.TasksServices;
using TaskManagerAPI.BL.UserStatusVerification;
using TaskManagerAPI.Helpers;
using TaskManagerAPI.Models.Errors;
using TaskManagerAPI.Repositories.AccountRepository;
using TaskManagerAPI.Repositories.TaskRepository;

namespace TaskManagerAPI.StartupConfiguration.Extensions
{
    /// <summary>
    /// Extended method of IServiceCollection to configure the BE services of the Service Layer. This aims to be a simple and minimum configuration. 
    /// </summary>
    public static class BeServicesExtension
    {
        public static IServiceCollection AddBeServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IAuthService, AuthService>();
            serviceCollection.AddTransient<ITokenCreator, TokenCreator>();
            serviceCollection.AddScoped<ITokenVerificator, TokenVerificator>();
            serviceCollection.AddScoped<IUserStatusVerification, UserStatusVerification>();
            serviceCollection.AddTransient<ICurrentUserTasksService, CurrentUserTasksService>();
            serviceCollection.AddTransient<ICurrentUserService, CurrentUserService>();

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
