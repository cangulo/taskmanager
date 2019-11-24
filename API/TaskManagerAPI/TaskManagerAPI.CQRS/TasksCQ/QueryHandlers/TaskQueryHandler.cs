using FluentResults;
using MediatR;
using System.Threading;
using TaskManagerAPI.BL.CurrentUserService;
using TaskManagerAPI.CQRS.TasksCQ.BaseClasses;
using TaskManagerAPI.CQRS.TasksCQ.Queries;
using TaskManagerAPI.Models.Errors;
using TaskManagerAPI.Repositories.TaskRepository;
using TaskManagerAPI.Resources.Errors;

namespace TaskManagerAPI.CQRS.TasksCQ.QueryHandlers
{
    public class TaskQueryHandler : BaseCommandQuery, IRequestHandler<TaskQuery, Result<Models.BE.Tasks.Task>>
    {
        private readonly ITasksByAccountRepository _tasksRepoByAccount;

        public TaskQueryHandler(ITasksByAccountRepository tasksRepoByAccount, ICurrentUserService currentUserService) : base(currentUserService)
        {
            _tasksRepoByAccount = tasksRepoByAccount;
        }

        public System.Threading.Tasks.Task<Result<Models.BE.Tasks.Task>> Handle(TaskQuery request, CancellationToken cancellationToken)
        {
            if (this._tasksRepoByAccount.TaskExists(this.GetCurrentUserId(), request.Id))
            {
                Models.BE.Tasks.Task task = this._tasksRepoByAccount.GetTask(this.GetCurrentUserId(), request.Id);
                Result<Models.BE.Tasks.Task> okResult = Results.Ok<Models.BE.Tasks.Task>(task);
                return System.Threading.Tasks.Task.FromResult(okResult);
            }
            else
            {
                return System.Threading.Tasks.Task.FromResult(Results.Fail<Models.BE.Tasks.Task>(
                    new ErrorCodeAndMessage(ErrorsCodesContants.TASK_ID_NOT_FOUND, ErrorsMessagesConstants.TASK_ID_NOT_FOUND)));
            }
        }
    }
}
