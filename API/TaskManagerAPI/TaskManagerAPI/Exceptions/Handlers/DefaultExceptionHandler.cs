using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using TaskManagerAPI.Models.Errors;
using TaskManagerAPI.Resources.Errors;

namespace TaskManagerAPI.Exceptions.Handlers
{
    public class DefaultExceptionHandler : IExceptionHandler
    {
        public string CreateResponseContent()
        {
            ErrorCodeAndMessage unkownError = new ErrorCodeAndMessage(
                    ErrorsCodesContants.UNKNOWN_ERROR_API, ErrorsMessagesConstants.UNKNOWN_ERROR_API);
            return JsonConvert.SerializeObject(unkownError);
        }

        public int GetHttpStatusCode()
        {
            return StatusCodes.Status500InternalServerError;
        }
    }
}
