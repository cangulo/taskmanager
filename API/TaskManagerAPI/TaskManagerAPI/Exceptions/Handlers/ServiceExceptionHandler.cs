using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using TaskManagerAPI.CQRS.Exceptions;
using TaskManagerAPI.Models.Errors;
using TaskManagerAPI.Resources.Errors;

namespace TaskManagerAPI.Exceptions.Handlers
{
    public class ServiceExceptionHandler : IExceptionHandler
    {
        private readonly IErrorToHttpStatusCodeHelper _errorCodeMapper;
        private readonly ServiceException _serviceException;

        public ServiceExceptionHandler(IErrorToHttpStatusCodeHelper errorCodeMapper, ServiceException serviceException)
        {
            _errorCodeMapper = errorCodeMapper;
            _serviceException = serviceException;
        }

        public Task AddErrorResponse(HttpResponse httpResponse)
        {
            string responseContent = string.Empty;
            if (_serviceException.Errors().Count > 0)
            {
                IEnumerable<string> errorCodes = _serviceException.Errors().Select(er => ((ErrorCodeAndMessage)er).Code);
                httpResponse.StatusCode = _errorCodeMapper.ToHttpStatusCode(errorCodes);
                List<ErrorCodeAndMessage> serviceErrors = _serviceException.Errors().Select(er => (ErrorCodeAndMessage)er).ToList();
                responseContent = JsonConvert.SerializeObject(serviceErrors);
            }
            else
            {
                ErrorCodeAndMessage unkownError = new ErrorCodeAndMessage(
                                    ErrorsCodesContants.UNKNOWN_ERROR_API, ErrorsMessagesConstants.UNKNOWN_ERROR_API);
                responseContent = JsonConvert.SerializeObject(unkownError);
                
            }
            return httpResponse.WriteAsync(responseContent);
        }
    }
}
