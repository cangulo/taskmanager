using System;
using System.Linq;
using TaskManagerAPI.EF.Context;
using TaskManagerAPI.Models.BE;
using TaskManagerAPI.Models.BE.Tasks;

namespace TaskManagerAPI.EF.DbInitializer
{
    public static class DummyDataHelper
    {
        public static Account Account = new Account()
        {
            Email = "john.doe@email.com",
            Username = "John Doe",
            Password = "WzkF3K5@4hU$",
            FailedLoginAttempts = 0,
            PhoneNumber = "+34670566831",
            LastLogintime = DateTime.MinValue,
            Status = UserStatus.Active
        };

        public static TaskDomain Task = new TaskDomain()
        {
            Title = "Remember to buy milk",
            Description = "We always forgot it right?",
            Account = DummyDataHelper.Account,
            AccountId = DummyDataHelper.Account.Id
        };

        public static void AddDummyAccount(TaskManagerDbContext dbContext)
        {
            dbContext.Accounts.Add(DummyDataHelper.Account);

        }
        public static void AddTaskAccount(TaskManagerDbContext dbContext)
        {
            DummyDataHelper.Task.AccountId = dbContext.Accounts.FirstOrDefault(a => a.Email == DummyDataHelper.Account.Email).Id;
            dbContext.Tasks.Add(DummyDataHelper.Task);

        }
    }
}