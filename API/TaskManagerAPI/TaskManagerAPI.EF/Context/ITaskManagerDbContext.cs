using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Models.BE;
using TaskManagerAPI.Models.BE.Tasks;

namespace TaskManagerAPI.EF.Context
{
    public interface ITaskManagerDbContext
    {
        DbSet<Account> Accounts { get; set; }
        DbSet<Task> Tasks{ get; set; }
        int SaveChanges();
    }
}
