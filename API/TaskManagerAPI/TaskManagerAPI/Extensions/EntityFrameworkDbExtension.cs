﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskManagerAPI.EF.Context;
using TaskManagerAPI.EF.DbInitializer;
using TaskManagerAPI.Resources.AppSettings;

namespace TaskManagerAPI.StartupConfiguration.Extensions
{
    /// <summary>
    /// Extended method of IServiceCollection to configure the DB using Entity Framework Core.
    /// Please note depending on the variable UseInMemoryDB, the application will use a in memory DB of EF or a dedicated SQL server DB.
    /// Also, if the DB is empty, in <see  cref="IDbInitializer"> IDbInitializer </see> a basic set of data will be added.
    /// <remarks>
    /// Execution of EF add-migration command:
    /// If this parameter is true the execution of the add-migration will fail because it couldn't do any migration to in memory db provider
    /// </remarks>
    /// </summary>
    /// TODO: Migrate to Autofac
    public static class EntityFrameworkDbExtension
    {
        public static IServiceCollection AddEFDBContext(this IServiceCollection serviceCollection, AppSettings appSettings, IConfiguration configuration)
        {
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
                        opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            }
            return serviceCollection;
        }
    }
}