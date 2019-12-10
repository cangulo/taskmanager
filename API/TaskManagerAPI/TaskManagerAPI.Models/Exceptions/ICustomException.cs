using FluentResults;
using System.Collections.Generic;
using TaskManagerAPI.Models.Errors;

namespace TaskManagerAPI.Models.Exceptions
{
    /// <summary>
    /// Custom Exception Model that handle the  <see cref="Error"> Error </see> model of <see cref="FluentResults.Error"> FluentResults </see>
    /// This aims to handle <see cref="TaskManagerAPI.Models.Errors.CustomError"> ErrorCodeAndMessage </see> as Error Type, please note it inherits from <see cref="Error"> Error </see>
    /// </summary>
    public interface ICustomException
    {
        IEnumerable<CustomError> Errors();
    }
}
