using Microsoft.Extensions.DependencyInjection;
using TaskManagerAPI.BL.AuthProcess;
using TaskManagerAPI.BL.CurrentUserService;
using TaskManagerAPI.BL.TasksServices;
using TaskManagerAPI.BL.UserStatusVerification;

namespace TaskManagerAPI.StartupConfiguration.Extensions
{
    /// <summary>
    /// Extended method of IServiceCollection to configure the BE services of the Service Layer. This aims to be a simple and minimum configuration. 
    /// </summary>
    public static class BeServicesExtension
    {
        public static IServiceCollection AddBeServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ITokenCreator, TokenCreator>();
            serviceCollection.AddScoped<ITokenVerificator, TokenVerificator>();
            serviceCollection.AddScoped<IUserStatusVerification, UserStatusVerification>();
            serviceCollection.AddTransient<ICurrentUserTasksService, CurrentUserTasksService>();
            serviceCollection.AddTransient<ICurrentUserService, CurrentUserService>();

            return serviceCollection;
        }
    }
}
