using Autofac;
using TaskManagerAPI.EF.Context;
using TaskManagerAPI.EF.DbInitializer;
using TaskManagerAPI.EF.MigrationManager;

namespace TaskManagerAPI.Extensions.AutofacModules
{
    public class EntityFrameworkModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<DBMigrationsManager>().As<IDBMigrationsManager>();
            containerBuilder.RegisterType<DbInitializer>().As<IDbInitializer>();
            containerBuilder.RegisterType<TaskManagerDbContext>().As<ITaskManagerDbContext>().SingleInstance();
        }
    }
}
