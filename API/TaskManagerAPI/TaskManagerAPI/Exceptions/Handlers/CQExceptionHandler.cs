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
        private readonly CQException _handlerException;

        public CQExceptionHandler(CQException handlerException)
        {
            // TODO: Test null input value
            if (handlerException == null || !handlerException.Errors().Any())
            {
                CustomError unkownError = new CustomError(ErrorsCodesContants.UNKNOWN_ERROR_API, ErrorsMessagesConstants.UNKNOWN_ERROR_API, 500);
                _handlerException = new CQException(new List<CustomError> { unkownError });

            }
            else
            {
                _handlerException = handlerException;
            }
        }

        public string CreateResponseContent()
        {
            List<CustomError> serviceErrors = _handlerException.Errors().Select(er => er).ToList();
            string responseContent = JsonConvert.SerializeObject(serviceErrors);
            return responseContent;
        }

        public int GetHttpStatusCode()
        {
            IEnumerable<int> errorCodes = _handlerException
                .Errors()
                .Select(er => er.Metadata.TryGetValue(ErrorKeyPropsConstants.ERROR_HTTP_CODE, out object erhttpCode) ? (int)erhttpCode : 500).OrderByDescending(value => value);
            return errorCodes.FirstOrDefault();
        }
    }
}