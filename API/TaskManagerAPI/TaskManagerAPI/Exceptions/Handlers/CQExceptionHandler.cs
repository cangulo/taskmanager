using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using TaskManagerAPI.CQRS.Exceptions;
using TaskManagerAPI.Models.Errors;
using TaskManagerAPI.Resources.Errors;

namespace TaskManagerAPI.Exceptions.Handlers
{
    public class CQExceptionHandler : IExceptionHandler
    {
        private readonly IErrorToHttpStatusCodeHelper _errorCodeMapper;
        private readonly CQException _handlerException;

        public CQExceptionHandler(IErrorToHttpStatusCodeHelper errorCodeMapper, CQException handlerException)
        {
            _errorCodeMapper = errorCodeMapper;
            _handlerException = handlerException;
        }

        public string CreateResponseContent()
        {
            string responseContent = string.Empty;
            if (_handlerException.Errors().Any())
            {
                List<CustomError> serviceErrors = _handlerException.Errors().Select(er => er).ToList();
                responseContent = JsonConvert.SerializeObject(serviceErrors);
            }
            else
            {
                CustomError unkownError = new CustomError(ErrorsCodesContants.UNKNOWN_ERROR_API, ErrorsMessagesConstants.UNKNOWN_ERROR_API, 500);
                responseContent = JsonConvert.SerializeObject(unkownError);
            }
            return responseContent;
        }

        public int GetHttpStatusCode()
        {
            if (_handlerException.Errors().Any())
            {
                // TODO: Solve
                //IEnumerable<string> errorCodes = _handlerException.Errors().Select(er => er.Code);
                //return _errorCodeMapper.ToHttpStatusCode(errorCodes);
                return 500;
            }
            else
            {
                return 500;
            }
        }
    }
}