using FluentResults;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TaskManagerAPI.BL.CurrentUserService;
using TaskManagerAPI.CQRS.TasksCQ.BaseClasses;
using TaskManagerAPI.CQRS.TasksCQ.Commands;
using TaskManagerAPI.Repositories.TaskRepository;

namespace TaskManagerAPI.CQRS.TasksCQ.CommandHandlers
{
    public class DeleteTaskCommandHandler : BaseCommandQuery, IRequestHandler<DeleteTaskCommand, Result>
    {
        private readonly ITasksByAccountRepository _tasksRepoByAccount;

        public DeleteTaskCommandHandler(ITasksByAccountRepository tasksRepoByAccount, ICurrentUserService currentUserService) : base(currentUserService)
        {
            _tasksRepoByAccount = tasksRepoByAccount;
        }

        public Task<Result> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            _tasksRepoByAccount.DeleteTask(this.GetCurrentUserId(), request.Id);
            return Task.FromResult(_tasksRepoByAccount.SaveModifications());
        }
    }
}