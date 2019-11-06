using FluentResults;
using System.Collections.Generic;
using TaskManagerAPI.Models.BE.Tasks;

namespace TaskManagerAPI.BL.TasksServices
{
    /// <summary>
    /// Service Layer for CRUD operations related to Tasks. This class is use <see cref="Repositories.TaskRepository.ITasksByAccountRepository"> ITasksByAccountRepository </see> to provide its services
    /// </summary>
    public interface ICurrentUserTasksService
    {
        Result CreateTask(Task task);
        Result DeleteTask(int taskId);
        Result<Task> GetTask(int taskId);
        List<Task> GetTasks();
        Result UpdateTask(int taskId, TaskForUpdated taskToBeFullUpdated);
    }
}