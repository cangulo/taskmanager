using System.Threading;
using System.Threading.Tasks;
using FluentResults;
using MediatR;
using TaskManagerAPI.BL.CurrentUserService;
using TaskManagerAPI.CQRS.TasksCQ.BaseClasses;
using TaskManagerAPI.CQRS.TasksCQ.Commands;
using TaskManagerAPI.Models.Errors;
using TaskManagerAPI.Repositories.TaskRepository;
using TaskManagerAPI.Resources.Errors;

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
            if (this._tasksRepoByAccount.TaskExists(this.GetCurrentUserId(), request.Id))
            {
                _tasksRepoByAccount.DeleteTask(this.GetCurrentUserId(), request.Id);
                return Task.FromResult(_tasksRepoByAccount.SaveModifications());
            }
            else
            {
                return Task.FromResult(Results.Fail(
                    new ErrorCodeAndMessage(ErrorsCodesContants.TASK_ID_NOT_FOUND, ErrorsMessagesConstants.TASK_ID_NOT_FOUND)));
            }
        }
    }
}
