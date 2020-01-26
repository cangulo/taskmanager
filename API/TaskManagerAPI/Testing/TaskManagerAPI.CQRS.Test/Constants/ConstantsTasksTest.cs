using System;
using TaskManagerAPI.Models.BE.Tasks;

namespace TaskManagerAPI.CQRS.Test.Contants
{
    public class ConstantsTasksTest
    {
        public const int Id = 1;
        public static TaskDomain taskDomain = new TaskDomain
        {
            Id = Id,
            Title = "Test Task Tittle",
            Description = "Test Task Description",
            DateToBeFinished = DateTime.MinValue,
            DateToBeNotified = DateTime.MinValue,
            Account = ConstantsAccountsCQTest.AccountTest,
            AccountId = ConstantsAccountsCQTest.AccountTest.Id
        };
    }
}