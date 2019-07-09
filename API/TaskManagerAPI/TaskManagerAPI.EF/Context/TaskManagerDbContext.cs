using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskManagerAPI.EF.Configurations;
using TaskManagerAPI.Models.BE;

namespace TaskManagerAPI.EF.Context
{
    public class TaskManagerDbContext : DbContext, ITaskManagerDbContext
    {
        /// <summary>
        /// https://docs.microsoft.com/es-es/ef/core/miscellaneous/configuring-dbcontext
        /// </summary>
        public DbSet<Account> Accounts { get; set; }
        private readonly ILogger _logger;
        public TaskManagerDbContext(DbContextOptions<TaskManagerDbContext> options, ILogger logger) : base(options)
        {
            _logger = logger;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            _logger.LogInformation("Configuring DB Context");
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            _logger.LogInformation("Configuring DB models");
            modelBuilder.ApplyConfiguration(new AccountEFConfiguration());
        }

        public override int SaveChanges()
        {
            _logger.LogInformation("Saving change in the DB");
            return base.SaveChanges();
        }
    }
}
