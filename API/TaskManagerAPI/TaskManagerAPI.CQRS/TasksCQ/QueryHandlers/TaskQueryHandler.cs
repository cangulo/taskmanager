using FluentResults;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TaskManagerAPI.BL.CurrentUserService;
using TaskManagerAPI.CQRS.TasksCQ.BaseClasses;
using TaskManagerAPI.CQRS.TasksCQ.Queries;
using TaskManagerAPI.Models.BE.Tasks;
using TaskManagerAPI.Models.Errors;
using TaskManagerAPI.Repositories.TaskRepository;
using TaskManagerAPI.Resources.Errors;

namespace TaskManagerAPI.CQRS.TasksCQ.QueryHandlers
{
    public class TaskQueryHandler : BaseCommandQuery, IRequestHandler<TaskQuery, Result<TaskDomain>>
    {
        private readonly ITasksByAccountRepository _tasksRepoByAccount;

        public TaskQueryHandler(ITasksByAccountRepository tasksRepoByAccount, ICurrentUserService currentUserService) : base(currentUserService)
        {
            _tasksRepoByAccount = tasksRepoByAccount;
        }

        public Task<Result<TaskDomain>> Handle(TaskQuery request, CancellationToken cancellationToken)
        {
            if (this._tasksRepoByAccount.TaskExists(this.GetCurrentUserId(), request.Id))
            {
                TaskDomain task = this._tasksRepoByAccount.GetTask(this.GetCurrentUserId(), request.Id);
                Result<TaskDomain> okResult = Results.Ok<TaskDomain>(task);
                return Task.FromResult(okResult);
            }
            else
            {
                return Task.FromResult(Results.Fail<TaskDomain>(
                    new CustomError(ErrorsCodesContants.TASK_ID_NOT_FOUND, ErrorsMessagesConstants.TASK_ID_NOT_FOUND, 404)));
            }
        }
    }
}
