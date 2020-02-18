using FluentResults;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TaskManagerAPI.CQRS.CustomDomainValidator;
using TaskManagerAPI.CQRS.DomainValidatorModel;
using TaskManagerAPI.CQRS.Exceptions;
using TaskManagerAPI.Models.Errors;
using TaskManagerAPI.Resources.Errors;

namespace TaskManagerAPI.CQRS.HandlerDecorator
{
    public class RequestHandlerValidatorDecorator<TRequest, TResult> : IRequestHandler<TRequest, TResult> where TRequest : IRequest<TResult> where TResult : ResultBase
    {
        private readonly IRequestHandler<TRequest, TResult> _decorated;
        private readonly ICustomDomainValidator<TRequest> _validator;

        public RequestHandlerValidatorDecorator(IRequestHandler<TRequest, TResult> decorated, ICustomDomainValidator<TRequest> validator)
        {
            _decorated = decorated;
            _validator = validator;
        }

        public async Task<TResult> Handle(TRequest request, CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(request);

            if (!validationResult.IsValid)
            {
                var validationFailure = (validationResult).Errors[0];
                if (validationFailure is CustomValidationFailure)
                {
                    CustomError customError = (validationFailure as CustomValidationFailure).CustomError;
                    List<CustomError> customErrors = new List<CustomError> { customError };
                    throw new CQException(customErrors);
                }
                else
                {
                    // 1. Create Generic Validator Errors for default Rules as greater than 0
                    // 2. Thrown a CQ Exception and encapsulate the errors there
                    // 3. Create a new CustomError for the validation of the request
                    return Results.Fail<TResult>(
                        new CustomError(ErrorsCodesContants.TASK_ID_NOT_FOUND, ErrorsMessagesConstants.TASK_ID_NOT_FOUND, 404)).Value;
                }
            }
            else
            {
                return await _decorated.Handle(request, cancellationToken);
            }
        }
    }

}