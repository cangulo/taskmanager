using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using TaskManagerAPI.Models.Errors;
using TaskManagerAPI.Resources.Errors;

namespace TaskManagerAPI.Exceptions.Handlers
{
    public class DefaultExceptionHandler : IExceptionHandler
    {

        public Task AddErrorResponse(HttpResponse httpResponse)
        {
            httpResponse.StatusCode = StatusCodes.Status500InternalServerError;
            ErrorCodeAndMessage unkownError = new ErrorCodeAndMessage(
                                ErrorsCodesContants.UNKNOWN_ERROR_API, ErrorsMessagesConstants.UNKNOWN_ERROR_API);
            string responseContent = JsonConvert.SerializeObject(unkownError);
            return httpResponse.WriteAsync(responseContent);
        }
    }
}
