using FluentResults;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TaskManagerAPI.BL.CurrentUserService;
using TaskManagerAPI.CQRS.TasksCQ.BaseClasses;
using TaskManagerAPI.CQRS.TasksCQ.Queries;
using TaskManagerAPI.Repositories.TaskRepository;

namespace TaskManagerAPI.CQRS.TasksCQ.QueryHandlers
{
    public class TaskCollectionQueryHandler : BaseCommandQuery, IRequestHandler<TaskCollectionQuery, Result<IReadOnlyCollection<Models.BE.Tasks.Task>>>
    {
        private readonly ITasksByAccountRepository _tasksRepoByAccount;

        public TaskCollectionQueryHandler(ITasksByAccountRepository tasksRepoByAccount, ICurrentUserService currentUserService) : base(currentUserService)
        {
            _tasksRepoByAccount = tasksRepoByAccount;
        }

        public Task<Result<IReadOnlyCollection<Models.BE.Tasks.Task>>> Handle(TaskCollectionQuery request, CancellationToken cancellationToken)
        {
            var task = _tasksRepoByAccount.GetTasks(this.GetCurrentUserId());
            Result<IReadOnlyCollection<Models.BE.Tasks.Task>> okResult = Results.Ok<IReadOnlyCollection<Models.BE.Tasks.Task>>(task);
            return Task.FromResult(okResult);
        }
    }
}
