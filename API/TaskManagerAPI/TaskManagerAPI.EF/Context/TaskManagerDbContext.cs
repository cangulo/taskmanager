using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskManagerAPI.EF.Configurations;
using TaskManagerAPI.Models.BE;
using TaskManagerAPI.Models.BE.Tasks;

namespace TaskManagerAPI.EF.Context
{
    public class TaskManagerDbContext : DbContext, ITaskManagerDbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Task> Tasks { get; set; }
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
            modelBuilder.ApplyConfiguration(new TaskEFConfiguration());
        }
        public override int SaveChanges()
        {
            _logger.LogInformation("Saving change in the DB");
            return base.SaveChanges();
        }
    }
}
