using AutoMapper;
using FluentResults;
using System.Collections.Generic;
using TaskManagerAPI.BL.CurrentUserService;
using TaskManagerAPI.Models.BE.Tasks;
using TaskManagerAPI.Models.Errors;
using TaskManagerAPI.Models.Exceptions;
using TaskManagerAPI.Repositories.TaskRepository;
using TaskManagerAPI.Resources.Errors;

namespace TaskManagerAPI.BL.TasksServices
{
    public class CurrentUserTasksService : ICurrentUserTasksService
    {
        private readonly int _currentUserId;
        private readonly IMapper _mapper;
        private readonly ITasksByAccountRepository _tasksRepoByAccount;
        public CurrentUserTasksService(ITasksByAccountRepository tasksRepoByAccount, ICurrentUserService currentUserService, IMapper mapper)
        {
            _tasksRepoByAccount = tasksRepoByAccount;
            _mapper = mapper;

            Result<int> opGetId = currentUserService.GetIdCurrentUser();
            if (opGetId.IsSuccess)
            {
                _currentUserId = opGetId.Value;
            }
            else
            {
                throw new CustomException(opGetId.Errors);
            }

        }

        public Result CreateTask(Task task)
        {
            _tasksRepoByAccount.CreateTask(_currentUserId, task);
            return _tasksRepoByAccount.SaveModifications();
        }

        public Result DeleteTask(int taskId)
        {
            if (this._tasksRepoByAccount.TaskExists(_currentUserId, taskId))
            {
                _tasksRepoByAccount.DeleteTask(_currentUserId, taskId);
                return _tasksRepoByAccount.SaveModifications();
            }
            else
            {
                return Results.Fail(
                    new ErrorCodeAndMessage(ErrorsCodesContants.TASK_ID_NOT_FOUND, ErrorsMessagesConstants.TASK_ID_NOT_FOUND));
            }
        }

        public Result<Task> GetTask(int taskId)
        {

            if (this._tasksRepoByAccount.TaskExists(_currentUserId, taskId))
            {
                return Results.Ok<Task>(this._tasksRepoByAccount.GetTask(_currentUserId, taskId));
            }
            else
            {
                return Results.Fail<Task>(
                    new ErrorCodeAndMessage(ErrorsCodesContants.TASK_ID_NOT_FOUND, ErrorsMessagesConstants.TASK_ID_NOT_FOUND));
            }
        }

        public List<Task> GetTasks()
        {
            return (List<Task>)this._tasksRepoByAccount.GetTasks(_currentUserId);
        }
        public Result UpdateTask(int taskId, TaskForUpdated taskToBeUpdated)
        {
            if (this._tasksRepoByAccount.TaskExists(_currentUserId, taskId))
            {
                Task taskInDb = _tasksRepoByAccount.GetTask(_currentUserId, taskId);
                _mapper.Map(taskToBeUpdated, taskInDb);
                return _tasksRepoByAccount.SaveModifications();
            }
            else
            {
                return Results.Fail(
                    new ErrorCodeAndMessage(ErrorsCodesContants.TASK_ID_NOT_FOUND, ErrorsMessagesConstants.TASK_ID_NOT_FOUND));
            }
        }
    }
}
