using FluentValidation.Results;

namespace TaskManagerAPI.CQRS.DomainValidatorModel
{
    public interface ICustomDomainValidator<TRequest>
    {
        ValidationResult Validate(TRequest instance);
    }
}
