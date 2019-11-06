using Microsoft.Extensions.Logging;
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
            if (_dbContext.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
            {
                _migrator.MigrateDB();
            }
            _dbContext.Database.EnsureCreated();
            _logger.LogInformation("DB has been initialized properly");
            if (!_dbContext.Accounts.Any())
            {
                _logger.LogInformation("creating basic users");
                DummyDataHelper.AddDummyAccount(_dbContext);
                _dbContext.SaveChanges();
                DummyDataHelper.AddTaskAccount(_dbContext);
                _dbContext.SaveChanges();
            }

        }
    }
}