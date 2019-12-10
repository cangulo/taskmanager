using FluentResults;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TaskManagerAPI.EF.Context;
using TaskManagerAPI.Models.BE;
using TaskManagerAPI.Models.BE.Tasks;
using TaskManagerAPI.Models.Errors;
using TaskManagerAPI.Resources.Errors;

namespace TaskManagerAPI.Repositories.TaskRepository
{
    public class TasksByAccountRepository : ITasksByAccountRepository
    {
        private readonly ITaskManagerDbContext _dbContext;

        public TasksByAccountRepository(ITaskManagerDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public TaskDomain GetTask(int accountId, int taskId)
        {
            return _dbContext.Tasks.FirstOrDefault(a => a.Id == taskId && a.AccountId == accountId);
        }
        public bool TaskExists(int accountId, int taskId)
        {
            return _dbContext.Tasks.Any(a => a.Id == taskId && a.AccountId == accountId);
        }
        public IReadOnlyCollection<TaskDomain> GetTasks(int accountId)
        {
            return new ReadOnlyCollection<TaskDomain>(_dbContext.Tasks.Where(a => a.AccountId == accountId).ToList());
        }
        public void DeleteTask(int accountId, int taskId)
        {
            TaskDomain taskToBeDeleted = _dbContext.Tasks.FirstOrDefault(t => t.AccountId == accountId && t.Id == taskId);
            _dbContext.Tasks.Remove(taskToBeDeleted);
        }
        public void CreateTask(int accountId, TaskDomain task)
        {
            Account account = _dbContext.Accounts.FirstOrDefault(a => a.Id == accountId);
            if (account != null)
            {
                task.Account = account;
                task.AccountId = accountId;
                _dbContext.Tasks.Add(task);
            }
        }
        public Result SaveModifications()
        {
            if (_dbContext.SaveChanges() >= 0)
            {
                return Results.Ok();
            }
            else
            {
                return Results.Fail(new CustomError(
                    ErrorsCodesContants.UNABLE_TO_SAVE_CHANGES_IN_TASK_TABLE,
                    ErrorsMessagesConstants.UNABLE_TO_SAVE_CHANGES_IN_TASK_TABLE, 500));
            }
        }
    }
}
