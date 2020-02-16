using FluentResults;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TaskManagerAPI.BL.CurrentUserService;
using TaskManagerAPI.CQRS.TasksCQ.BaseClasses;
using TaskManagerAPI.CQRS.TasksCQ.Commands;
using TaskManagerAPI.CQRS.TasksCQ.CommandValidators;
using TaskManagerAPI.Models.Errors;
using TaskManagerAPI.Repositories.TaskRepository;
using TaskManagerAPI.Resources.Errors;

namespace TaskManagerAPI.CQRS.TasksCQ.CommandHandlers
{
    public class DeleteTaskCommandHandler : BaseCommandQuery, IRequestHandler<DeleteTaskCommand, Result>
    {
        private readonly ITasksByAccountRepository _tasksRepoByAccount;
        private readonly DeleteTaskCommandValidator _validator;

        public DeleteTaskCommandHandler(ITasksByAccountRepository tasksRepoByAccount, ICurrentUserService currentUserService, DeleteTaskCommandValidator validator) : base(currentUserService)
        {
            _tasksRepoByAccount = tasksRepoByAccount;
            _validator = validator;
        }
        
        public Task<Result> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            if (!this._validator.Validate(request).IsValid)
            {
                return Task.FromResult(Results.Fail(
                    new CustomError(ErrorsCodesContants.TASK_ID_NOT_FOUND, ErrorsMessagesConstants.TASK_ID_NOT_FOUND, 404)));
            }
            if (this._tasksRepoByAccount.TaskExists(this.GetCurrentUserId(), request.Id))
            {
                _tasksRepoByAccount.DeleteTask(this.GetCurrentUserId(), request.Id);
                return Task.FromResult(_tasksRepoByAccount.SaveModifications());
            }
            else
            {
                return Task.FromResult(Results.Fail(
                    new CustomError(ErrorsCodesContants.TASK_ID_NOT_FOUND, ErrorsMessagesConstants.TASK_ID_NOT_FOUND, 404)));
            }
        }
    }
}