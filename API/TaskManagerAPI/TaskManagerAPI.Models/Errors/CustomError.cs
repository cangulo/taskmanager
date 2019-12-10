using FluentResults;

namespace TaskManagerAPI.Models.Errors
{
    /// <summary>
    /// Custom class to encapsulate errors in the <see cref="FluentResults.Results.Fail(Error)">Results.Fail</see> method
    /// In that way, in case of error, anywhere we get the  <see cref="FluentResults.Result"/> type, we can get this error object attached.
    /// </summary>
    public class CustomError : Error
    {
        public CustomError(string errorCode, string errorMessage, int httpCode) : base(errorMessage)
        {
            this.Metadata.Add(ErrorKeyPropsConstants.ERROR_CODE, errorCode);
            this.Metadata.Add(ErrorKeyPropsConstants.ERROR_HTTP_CODE, httpCode);
        }

        public override string ToString()
        {
            this.Metadata.TryGetValue(ErrorKeyPropsConstants.ERROR_CODE, out object errorCode);
            return $"{(string)errorCode};{base.Message}";
        }
    }
}
