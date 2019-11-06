using FluentResults;

namespace TaskManagerAPI.Models.Errors
{
    /// <summary>
    /// Custom class to encapsulate errors in the <see cref="FluentResults.Results.Fail(Error)">Results.Fail</see> method
    /// In that way, in case of error, anywhere we get the  <see cref="FluentResults.Result"/> type, we can get this error object attached.
    /// </summary>
    public class ErrorCodeAndMessage : Error
    {
        // We set the Error Code as public to allow that FE receive the code 
        // and use it to handle any specific unexpected situation
        public string Code { get; }

        public ErrorCodeAndMessage(string errorCode, string errorMessage) : base(errorMessage)
        {
            Code = errorCode; ;
        }

        public override string ToString()
        {
            return $"{Code};{base.Message}";
        }
    }
}
