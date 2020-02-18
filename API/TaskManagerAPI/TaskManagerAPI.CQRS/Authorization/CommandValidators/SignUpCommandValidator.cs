using FluentValidation;
using TaskManagerAPI.CQRS.Authorization.Commands;
using TaskManagerAPI.CQRS.CustomDomainValidator;
using TaskManagerAPI.CQRS.DomainValidatorModel;
using TaskManagerAPI.Models.Errors;
using TaskManagerAPI.Repositories.AccountRepository;
using TaskManagerAPI.Resources.Errors;

namespace TaskManagerAPI.CQRS.Authorization.CommandValidators
{
    public class SignUpCommandValidator : BaseCustomDomainValidator<SignUpCommand>
    {
        public SignUpCommandValidator(IAccountRepository accountRepository)
        {
            RuleFor(m => m.Email)
                .Custom((email, context) =>
                {
                    if (accountRepository.ExistsAccount(email))
                    {
                        CustomError customError = new CustomError(ErrorsCodesContants.EMAIL_ALREADY_USED, ErrorsMessagesConstants.EMAIL_ALREADY_USED, 400);
                        CustomValidationFailure customFailure = new CustomValidationFailure(customError, "email");
                        context.AddFailure(customFailure);
                    }
                });
        }
    }
}
