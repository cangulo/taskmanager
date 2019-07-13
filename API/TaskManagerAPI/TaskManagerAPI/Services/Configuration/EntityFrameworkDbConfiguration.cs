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
        public static IServiceCollection AddEntityFrameworkDbConfiguration(this IServiceCollection serviceCollection, AppSettings appSettings)
        {
            // EF add-migration command: If this parameter is true the execution of the add-migration will fail because it couldn't do any migration to in memory db provider
            bool useInMemoryDB = appSettings.UseInMemoryDB;
            if (useInMemoryDB)
            {
                serviceCollection.
                    AddDbContext<TaskManagerDbContext>(opt => opt.UseInMemoryDatabase(databaseName: "TaskManagerApi"));
            }
            else
            {
                serviceCollection.
                    AddEntityFrameworkSqlServer().
                    AddDbContext<TaskManagerDbContext>(opt =>
                        opt.UseSqlServer(appSettings.ConnectionString));
            }

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
