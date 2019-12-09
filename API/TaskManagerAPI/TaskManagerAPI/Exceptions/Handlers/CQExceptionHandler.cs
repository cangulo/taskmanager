using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
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
                List<ErrorCodeAndMessage> serviceErrors = _handlerException.Errors().Select(er => er).ToList();
                responseContent = JsonConvert.SerializeObject(serviceErrors);
            }
            else
            {
                ErrorCodeAndMessage unkownError = new ErrorCodeAndMessage(
                                    ErrorsCodesContants.UNKNOWN_ERROR_API, ErrorsMessagesConstants.UNKNOWN_ERROR_API);
                responseContent = JsonConvert.SerializeObject(unkownError);

            }
            return responseContent;
        }

        public int GetHttpStatusCode()
        {
            if (_handlerException.Errors().Any())
            {
                IEnumerable<string> errorCodes = _handlerException.Errors().Select(er => er.Code);
                return _errorCodeMapper.ToHttpStatusCode(errorCodes);
            }
            else
            {
                return 500;
            }
        }
    }
}
