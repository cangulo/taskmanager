using Autofac;
using TaskManagerAPI.Repositories.AccountRepository;
using TaskManagerAPI.Repositories.TaskRepository;

namespace TaskManagerAPI.Extensions.AutofacModules
{
    public class RepositoriesModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<AccountRepository>().As<IAccountRepository>();
            containerBuilder.RegisterType<TasksByAccountRepository>().As<ITasksByAccountRepository>();
        }
    }
}
