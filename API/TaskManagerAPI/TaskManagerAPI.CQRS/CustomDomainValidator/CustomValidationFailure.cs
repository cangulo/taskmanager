using FluentValidation.Results;
using TaskManagerAPI.Models.Errors;

namespace TaskManagerAPI.CQRS.CustomDomainValidator
{
    public class CustomValidationResult : ValidationResult
    {

    }
    public class CustomValidationFailure : ValidationFailure
    {
        public CustomError CustomError { get; set; }
        public CustomValidationFailure(CustomError customError, string property) : base(property, customError.Message)
        {
            this.CustomError = customError;
        }
    }
}
