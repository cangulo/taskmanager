using FluentValidation;
using TaskManagerAPI.BL.CurrentUserService;
using TaskManagerAPI.CQRS.CustomDomainValidator;
using TaskManagerAPI.CQRS.DomainValidatorModel;
using TaskManagerAPI.CQRS.TasksCQ.Commands;
using TaskManagerAPI.Models.Errors;
using TaskManagerAPI.Repositories.TaskRepository;
using TaskManagerAPI.Resources.Errors;

namespace TaskManagerAPI.CQRS.TasksCQ.CommandValidators
{
    public class DeleteTaskCommandValidator : BaseCustomDomainValidator<DeleteTaskCommand>
    {
        public DeleteTaskCommandValidator(ITasksByAccountRepository tasksRepoByAccount, ICurrentUserService currentUserService)
        {
            RuleFor(m => m.Id)
                .GreaterThan(0)
                .Custom((id, context) =>
                {
                    var currentUserIdResult = currentUserService.GetIdCurrentUser();
                    if (currentUserIdResult.IsSuccess)
                    {
                        if (!tasksRepoByAccount.TaskExists(currentUserIdResult.Value, id))
                        {
                            CustomError customError = new CustomError(ErrorsCodesContants.TASK_ID_NOT_FOUND, ErrorsMessagesConstants.TASK_ID_NOT_FOUND, 404);
                            CustomValidationFailure customFailure = new CustomValidationFailure(customError, "task id");
                            context.AddFailure(customFailure);
                        }
                    }
                    else
                    {
                        CustomValidationFailure customFailure = new CustomValidationFailure(currentUserIdResult.Errors[0] as CustomError, "current user");
                        context.AddFailure(customFailure);
                    }
                });
        }
    }
}
