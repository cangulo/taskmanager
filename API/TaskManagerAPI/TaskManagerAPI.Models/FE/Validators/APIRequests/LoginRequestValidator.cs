using FluentValidation;
using TaskManagerAPI.Models.FE.APIRequests;
using TaskManagerAPI.Resources.Constants;

namespace TaskManagerAPI.Models.FE.Validators.APIRequests
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(m => m.Email).EmailAddress().MaximumLength(AttributesContants.MAX_LENGTH_EMAIL);
            RuleFor(m => m.Password).Matches(RegularExpressionsConstants.PASSWORD_FORMAT).MaximumLength(AttributesContants.MAX_LENGTH_PASSWORD);
        }
    }
}
