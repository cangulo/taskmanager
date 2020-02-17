using FluentResults;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TaskManagerAPI.BL.CurrentUserService;
using TaskManagerAPI.CQRS.CustomDomainValidator;
using TaskManagerAPI.CQRS.DomainValidatorModel;
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
        private readonly ICustomDomainValidator<DeleteTaskCommand> _validator;

        public DeleteTaskCommandHandler(ITasksByAccountRepository tasksRepoByAccount, ICurrentUserService currentUserService, ICustomDomainValidator<DeleteTaskCommand> validator) : base(currentUserService)
        {
            _tasksRepoByAccount = tasksRepoByAccount;
            _validator = validator;
        }

        public Task<Result> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            var validationResult = this._validator.Validate(request);
            if (!validationResult.IsValid)
            {
                var validationFailure = (validationResult).Errors[0];
                if (validationFailure is CustomValidationFailure)
                {
                    CustomError customError = (validationFailure as CustomValidationFailure).CustomError;
                    return Task.FromResult(Results.Fail(customError));
                }
                else
                {
                    return Task.FromResult(Results.Fail(
                    new CustomError(ErrorsCodesContants.TASK_ID_NOT_FOUND, ErrorsMessagesConstants.TASK_ID_NOT_FOUND, 404)));
                }
            }

            _tasksRepoByAccount.DeleteTask(this.GetCurrentUserId(), request.Id);
            return Task.FromResult(_tasksRepoByAccount.SaveModifications());
        }
    }
}