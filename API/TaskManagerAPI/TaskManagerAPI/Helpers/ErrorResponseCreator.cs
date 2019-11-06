using FluentResults;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using TaskManagerAPI.Models.Errors;
using TaskManagerAPI.Resources.Errors;

namespace TaskManagerAPI.Helpers
{
    public class ErrorResponseCreator : IErrorResponseCreator
    {
        private readonly IErrorToHttpStatusCodeHelper errorCodeMapper;
        public ErrorResponseCreator(IErrorToHttpStatusCodeHelper errorCodeMapper)
        {
            this.errorCodeMapper = errorCodeMapper;
        }
        public IActionResult CreateResponse(List<Error> errors)
        {
            ObjectResult actionResult;
            if (errors.Count > 0)
            {
                if (errors is List<Error> && errors.TrueForAll(er => er.GetType() == typeof(ErrorCodeAndMessage)))
                {
                    List<ErrorCodeAndMessage> appErrors = errors.Select(er => (ErrorCodeAndMessage)er).ToList();
                    actionResult = (new ObjectResult(appErrors));
                    int statusCode = this.errorCodeMapper.ToHttpStatusCode(appErrors.Select(er => er.Code));
                    actionResult.StatusCode = statusCode;
                }
                else
                {
                    actionResult = (new ObjectResult(errors));
                    actionResult.StatusCode = 500;
                }


            }
            else
            {
                ErrorCodeAndMessage unkownError = new ErrorCodeAndMessage(
                                    ErrorsCodesContants.UNKNOWN_ERROR_API, ErrorsMessagesConstants.UNKNOWN_ERROR_API);
                actionResult = (new ObjectResult(unkownError));
                actionResult.StatusCode = 500;
            }

            return actionResult;
        }
    }
}
