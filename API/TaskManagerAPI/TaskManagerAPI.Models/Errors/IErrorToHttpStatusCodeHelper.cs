using System.Collections.Generic;

namespace TaskManagerAPI.Models.Errors
{
    public interface IErrorToHttpStatusCodeHelper
    {
        int ToHttpStatusCode(string errorCode);
        int ToHttpStatusCode(IEnumerable<string> errorCodes);
    }
}
