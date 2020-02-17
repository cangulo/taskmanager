using FluentValidation;

namespace TaskManagerAPI.CQRS.DomainValidatorModel
{
    public abstract class BaseCustomDomainValidator<TRequest> : AbstractValidator<TRequest>, ICustomDomainValidator<TRequest>
    {
    }
}
