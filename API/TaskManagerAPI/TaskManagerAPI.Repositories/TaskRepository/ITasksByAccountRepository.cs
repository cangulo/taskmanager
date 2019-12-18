using FluentResults;
using System.Collections.Generic;
using TaskManagerAPI.Models.BE.Tasks;

namespace TaskManagerAPI.Repositories.TaskRepository
{
    /// <summary>
    /// Repository Class for CRUD operations related to Task table. All operations required the id of the account associated with the tasks.
    /// </summary>
    public interface ITasksByAccountRepository
    {
        IReadOnlyCollection<TaskDomain> GetTasks(int accountId);

        bool TaskExists(int accountId, int taskId);

        TaskDomain GetTask(int accountId, int taskId);

        void DeleteTask(int accountId, int taskId);

        void CreateTask(int accountId, TaskDomain task);

        Result SaveModifications();
    }
}