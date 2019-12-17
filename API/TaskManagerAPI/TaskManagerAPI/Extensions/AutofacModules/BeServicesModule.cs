using Autofac;
using TaskManagerAPI.BL.AuthProcess;
using TaskManagerAPI.BL.CurrentUserService;
using TaskManagerAPI.BL.UserStatusVerification;

namespace TaskManagerAPI.Extensions.AutofacModules
{
    public class BeServicesModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<TokenCreator>().As<ITokenCreator>();
            containerBuilder.RegisterType<TokenVerificator>().As<ITokenVerificator>();
            containerBuilder.RegisterType<UserStatusVerification>().As<IUserStatusVerification>();
            containerBuilder.RegisterType<CurrentUserService>().As<ICurrentUserService>();
        }
    }
}
