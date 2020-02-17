using FluentValidation.Results;
using System.Collections.Generic;
using TaskManagerAPI.Models.Errors;

namespace TaskManagerAPI.CQRS.CustomDomainValidator
{
    public class CustomValidationResult : ValidationResult
    {
        public CustomValidationResult(IList<CustomValidationFailure> Errors) : base(Errors)
        {
        }
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
