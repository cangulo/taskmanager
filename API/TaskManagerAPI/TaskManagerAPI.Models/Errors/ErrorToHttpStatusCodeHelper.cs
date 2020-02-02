using System.Collections.Generic;
using System.Linq;
using TaskManagerAPI.Resources.Errors;

namespace TaskManagerAPI.Models.Errors
{
    public class ErrorToHttpStatusCodeHelper : IErrorToHttpStatusCodeHelper
    {
        /// <summary>
        /// Map Status Codes based on https://developer.mozilla.org/es/docs/Web/HTTP/Status
        /// </summary>
        /// <param name="errorCode"></param>
        /// <returns></returns>
        public int ToHttpStatusCode(string errorCode)
        {
            if (!string.IsNullOrEmpty(errorCode) && errorCode.Length >= 5)
            {
                if (int.TryParse(errorCode[0].ToString(), out int layer))
                {
                    switch (layer)
                    {
                        case (int)ErrorLayer.API:
                            return 500;

                        case (int)ErrorLayer.BL:
                            if (int.TryParse(errorCode[1].ToString(), out int typeOfError))
                            {
                                switch (typeOfError)
                                {
                                    case 4:
                                        switch (errorCode)
                                        {
                                            case ErrorsCodesContants.INVALID_EMAIL_OR_PASSWORD:
                                                return 401;
                                            case ErrorsCodesContants.TASK_ID_NOT_FOUND:
                                                return 404;
                                            default:
                                                return 400;
                                        }
                                }
                            }
                            return 500;
                    }
                }
            }
            return 500; // Internal Server Error
        }

        /// <summary>
        /// 500 status codes has priority over other error response
        /// </summary>
        /// <param name="errorCodes"></param>
        /// <returns></returns>
        public int ToHttpStatusCode(IEnumerable<string> errorCodes)
        {
            List<int> statusCodes = errorCodes.Select(errorCode => this.ToHttpStatusCode(errorCode)).ToList();
            if (statusCodes.Count != 1 && statusCodes.Any(code => code >= 500))
            {
                return statusCodes.First(code => code >= 500);
            }
            else
            {
                return statusCodes.First();
            }
        }
    }
}