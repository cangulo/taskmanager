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
            CustomError unkownError = new CustomError(ErrorsCodesContants.UNKNOWN_ERROR_API, ErrorsMessagesConstants.UNKNOWN_ERROR_API, 500);
            return JsonConvert.SerializeObject(unkownError);
        }

        public int GetHttpStatusCode()
        {
            return StatusCodes.Status500InternalServerError;
        }
    }
}
