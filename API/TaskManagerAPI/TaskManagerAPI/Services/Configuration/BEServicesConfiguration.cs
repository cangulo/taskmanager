using Microsoft.Extensions.DependencyInjection;
using TaskManagerAPI.BL.AuthProcess;
using TaskManagerAPI.BL.UserVerifications;

namespace TaskManagerAPI.Services.Configuration
{
    public static class BEServicesConfiguration
    {
        public static IServiceCollection AddBEServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IAuthService, AuthService>();
            serviceCollection.AddScoped<ITokenCreator, TokenCreator>();
            serviceCollection.AddScoped<ITokenVerificator, TokenVerificator>();
            serviceCollection.AddScoped<IUserVerification, UserVerification>();

            return serviceCollection;
        }
    }
}
