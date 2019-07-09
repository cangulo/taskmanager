using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using TaskManagerAPI.EF.Context;
using TaskManagerAPI.EF.MigrationManager;

namespace TaskManagerAPI.EF.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly TaskManagerDbContext _dbContext;

        private readonly IDBMigrationsManager _migrator;
        private readonly ILogger _logger;

        public DbInitializer(TaskManagerDbContext dbContext, IDBMigrationsManager migrator, ILogger<DbInitializer> logger)
        {
            _dbContext = dbContext;
            _migrator = migrator;
            _logger = logger;
        }

        public void StartDbContext()
        {
            _logger.LogInformation("Starting DB");
            _migrator.MigrateDB();
            _dbContext.Database.EnsureCreated();
            _logger.LogInformation("DB has been initializated properly");
            if (!_dbContext.Accounts.Any())
            {
                _logger.LogInformation("creating basic users");
                _dbContext.Accounts.Add(
                    new Models.BE.Account()
                    {
                        Email = "carlos.angulo.mascarell@outlook.com",
                        Username = "Carlos Angulo",
                        Password = "gochocatalan",
                        FailedLoginAttempts = 0,
                        PhoneNumber = "+34670566831",
                        LastLogintime = DateTime.MinValue
                    });
                _dbContext.SaveChanges();
            }

        }
    }
}