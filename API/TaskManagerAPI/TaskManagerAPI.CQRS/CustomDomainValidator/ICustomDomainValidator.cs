using FluentValidation.Results;
using TaskManagerAPI.CQRS.CustomDomainValidator;

namespace TaskManagerAPI.CQRS.DomainValidatorModel
{
    public interface ICustomDomainValidator<TRequest>
    {
        ValidationResult Validate(TRequest instance);
    }
}
