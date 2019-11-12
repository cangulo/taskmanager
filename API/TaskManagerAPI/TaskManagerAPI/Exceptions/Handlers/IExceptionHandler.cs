using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace TaskManagerAPI.Exceptions.Handlers
{
    public interface IExceptionHandler
    {
        Task AddErrorResponse(HttpResponse httpResponse);

    }
}
