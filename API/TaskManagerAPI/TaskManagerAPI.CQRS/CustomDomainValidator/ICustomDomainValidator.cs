using FluentValidation.Results;

namespace TaskManagerAPI.CQRS.DomainValidatorModel
{
    // Not used yet
    public interface ICustomDomainValidator<TValidator,TRequest>
    {
        ValidationResult Validate(TRequest request);
    }
}
