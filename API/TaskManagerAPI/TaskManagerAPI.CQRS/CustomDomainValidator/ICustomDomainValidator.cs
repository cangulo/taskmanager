using FluentValidation;
using FluentValidation.Results;

namespace TaskManagerAPI.CQRS.DomainValidatorModel
{
    public interface ICustomDomainValidator<in TRequest>
    {
        ValidationResult Validate(TRequest instance);
    }
    //public abstract class BaseCustomDomainValidator<TRequest>
    //{
    //    CustomValidationResult ICustomDomainValidator<TRequest>.Validate(TRequest request)
    //    {
    //        return this.Validate(request) as CustomValidationResult;
    //    }
    //}
}
