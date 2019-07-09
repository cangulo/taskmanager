using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Models.BE;

namespace TaskManagerAPI.EF.Context
{
    public interface ITaskManagerDbContext
    {
        DbSet<Account> Accounts { get; set; }
        int SaveChanges();
    }
}
