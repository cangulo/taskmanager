using FluentValidation;

namespace TaskManagerAPI.CQRS.DomainValidatorModel
{
    public abstract class BaseCustomDomainValidator<TRequest> : AbstractValidator<TRequest>, ICustomDomainValidator<TRequest>
    {
        //public ValidationResult Validate(TRequest request)
        //{
        //    var result = base.Validate(request);
        //    var customResult = (CustomValidationResult)result;
        //    return customResult;
        //}
    }
}
