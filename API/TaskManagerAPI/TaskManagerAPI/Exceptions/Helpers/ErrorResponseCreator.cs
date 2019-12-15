using FluentResults;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using TaskManagerAPI.Models.Errors;
using TaskManagerAPI.Resources.Errors;

namespace TaskManagerAPI.Exceptions.Helpers
{
    public class ErrorResponseCreator : IErrorResponseCreator
    {
        public IActionResult CreateResponse(List<Error> errors)
        {
            ObjectResult actionResult;
            if (errors.Count == 0 || errors.Any(err => err.Metadata.TryGetValue(ErrorKeyPropsConstants.ERROR_HTTP_CODE, out object httpErroCode) && (int)err.Metadata[ErrorKeyPropsConstants.ERROR_HTTP_CODE] == 500))
            {
                CustomError unkownError = new CustomError(ErrorsCodesContants.UNKNOWN_ERROR_API, ErrorsMessagesConstants.UNKNOWN_ERROR_API, 500);
                actionResult = (new ObjectResult(unkownError));
                actionResult.StatusCode = 500;
            }
            else
            {
                if (errors is List<Error> && errors.TrueForAll(er => er.GetType() == typeof(CustomError)))
                {
                    List<CustomError> appErrors = errors.Select(er => (CustomError)er).ToList();
                    actionResult = (new ObjectResult(appErrors));
                    // TODO: Solve 
                    //int statusCode = this.errorCodeMapper.ToHttpStatusCode(appErrors.Select(er => er.Code));
                    //actionResult.StatusCode = statusCode;
                    actionResult.StatusCode = 500;

                }
                else
                {
                    actionResult = (new ObjectResult(errors));
                    actionResult.StatusCode = 500;
                }
            }
            return actionResult;
        }
    }
}
