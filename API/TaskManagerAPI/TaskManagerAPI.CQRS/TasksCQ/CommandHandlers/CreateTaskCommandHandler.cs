using System.Threading;
using System.Threading.Tasks;
using FluentResults;
using MediatR;
using TaskManagerAPI.BL.CurrentUserService;
using TaskManagerAPI.CQRS.TasksCQ.BaseClasses;
using TaskManagerAPI.CQRS.TasksCQ.Commands;
using TaskManagerAPI.Repositories.TaskRepository;

namespace TaskManagerAPI.CQRS.TasksCQ.CommandHandlers
{
    public partial class CreateTaskCommandHandler : BaseCommandQuery, IRequestHandler<CreateTaskCommand, Result>
    {
        private readonly ITasksByAccountRepository _tasksRepoByAccount;

        public CreateTaskCommandHandler(ITasksByAccountRepository tasksRepoByAccount, ICurrentUserService currentUserService) : base(currentUserService)
        {

            _tasksRepoByAccount = tasksRepoByAccount;
        }

        public Task<Result> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            _tasksRepoByAccount.CreateTask(this.GetCurrentUserId(), request.Task);
            return Task.FromResult(_tasksRepoByAccount.SaveModifications());
        }
    }
}