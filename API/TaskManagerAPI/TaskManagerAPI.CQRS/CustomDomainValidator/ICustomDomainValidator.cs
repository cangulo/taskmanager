using FluentValidation.Results;

namespace TaskManagerAPI.CQRS.DomainValidatorModel
{
    public interface ICustomDomainValidator<TValidator,TRequest>
    {
        ValidationResult Validate(TRequest request);
    }
}
