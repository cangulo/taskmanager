using FluentValidation;
using TaskManagerAPI.Models.FE.APIRequests;
using TaskManagerAPI.Resources.Constants;

namespace TaskManagerAPI.Models.FE.Validators.APIRequests
{
    public class SignUpRequestValidator : AbstractValidator<SignUpRequest>
    {
        public SignUpRequestValidator()
        {
            RuleFor(m => m.FullName).MinimumLength(AttributesContants.MIN_LENGTH_FULLNAME).MaximumLength(AttributesContants.MAX_LENGTH_FULLNAME);
            RuleFor(m => m.Email)
                .EmailAddress().WithMessage("Please provide a valid email")
                .MaximumLength(AttributesContants.MAX_LENGTH_EMAIL);
            RuleFor(m => m.Password).Matches(RegularExpressionsConstants.PASSWORD_FORMAT).MaximumLength(AttributesContants.MAX_LENGTH_PASSWORD);
        }
    }
}
