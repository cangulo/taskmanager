using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TaskManagerAPI.EF.Context;
using TaskManagerAPI.EF.DbInitializer;
using TaskManagerAPI.EF.MigrationManager;
using TaskManagerAPI.Resources.AppSettings;

namespace TaskManagerAPI.Services.Configuration
{
    public static class EntityFrameworkDbConfiguration
    {
        public static IServiceCollection AddEntityFrameworkDbConfiguration(this IServiceCollection serviceCollection)
        {
            #region EF migrations commants
            // Entity Framework create intial db migration
            // add-migration InitialMigration
            #endregion

            serviceCollection.
                AddEntityFrameworkSqlServer().
                AddDbContext<TaskManagerDbContext>(opt =>
                    opt.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TaskManagerApi;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"));

            serviceCollection.AddTransient<IDBMigrationsManager, DBMigrationsManager>((ctx) =>
            {
                return new DBMigrationsManager(ctx.GetService<TaskManagerDbContext>(), ctx.GetService<ILogger<DBMigrationsManager>>());
            });
            serviceCollection.AddTransient<IDbInitializer, DbInitializer>((ctx) =>
            {
                return new DbInitializer(ctx.GetService<TaskManagerDbContext>(), ctx.GetService<IDBMigrationsManager>(), ctx.GetService<ILogger<DbInitializer>>());
            });
            serviceCollection.AddScoped<ITaskManagerDbContext, TaskManagerDbContext>();

            return serviceCollection;
        }
    }
}
