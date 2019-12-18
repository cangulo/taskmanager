using FluentResults;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace TaskManagerAPI.Exceptions.Helpers
{
    /// <summary>
    /// Class to create a HTTP response containing error objects of type <see cref="FluentResults.Error">FluentResults.Error</see>
    /// or the ones that inherit from it, as <see cref="TaskManagerAPI.Models.Errors.CustomError">ErrorCodeAndMessage</see> class
    /// </summary>
    public interface IErrorResponseCreator
    {
        IActionResult CreateResponse(List<Error> errors);
    }
}