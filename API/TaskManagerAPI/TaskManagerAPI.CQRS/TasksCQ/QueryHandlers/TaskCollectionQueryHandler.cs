using FluentResults;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TaskManagerAPI.BL.CurrentUserService;
using TaskManagerAPI.CQRS.TasksCQ.BaseClasses;
using TaskManagerAPI.CQRS.TasksCQ.Queries;
using TaskManagerAPI.Models.BE.Tasks;
using TaskManagerAPI.Repositories.TaskRepository;

namespace TaskManagerAPI.CQRS.TasksCQ.QueryHandlers
{
    public class TaskCollectionQueryHandler : BaseCommandQuery, IRequestHandler<TaskCollectionQuery, Result<IReadOnlyCollection<TaskDomain>>>
    {
        private readonly ITasksByAccountRepository _tasksRepoByAccount;

        public TaskCollectionQueryHandler(ITasksByAccountRepository tasksRepoByAccount, ICurrentUserService currentUserService) : base(currentUserService)
        {
            _tasksRepoByAccount = tasksRepoByAccount;
        }

        public Task<Result<IReadOnlyCollection<TaskDomain>>> Handle(TaskCollectionQuery request, CancellationToken cancellationToken)
        {
            var task = _tasksRepoByAccount.GetTasks(this.GetCurrentUserId());
            Result<IReadOnlyCollection<TaskDomain>> okResult = Results.Ok<IReadOnlyCollection<TaskDomain>>(task);
            return Task.FromResult(okResult);
        }
    }
}