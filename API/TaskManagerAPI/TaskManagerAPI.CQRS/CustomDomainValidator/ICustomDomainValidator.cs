//using FluentValidation;
//using TaskManagerAPI.CQRS.CustomDomainValidator;

//namespace TaskManagerAPI.CQRS.DomainValidatorModel
//{
//    public interface ICustomDomainValidator<TRequest>
//    {
//        abstract CustomValidationResult Validate(TRequest request);
//    }
//    public abstract class BaseCustomDomainValidator<TRequest> 
//    {
//        CustomValidationResult ICustomDomainValidator<TRequest>.Validate(TRequest request)
//        {
//            return this.Validate(request) as CustomValidationResult;
//        }
//    }
//}
