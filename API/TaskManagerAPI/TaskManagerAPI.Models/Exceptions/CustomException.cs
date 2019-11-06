using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskManagerAPI.Models.Exceptions
{
    /// <summary>
    /// Custom Exception Model that handle the  <see cref="Error"> Error </see> model of <see cref="FluentResults.Error"> FluentResults </see>
    /// This aims to also handle <see cref="TaskManagerAPI.Models.Errors.ErrorCodeAndMessage"> ErrorCodeAndMessage </see> because it inherit from <see cref="Error"> Error </see>
    /// </summary>
    public class CustomException : Exception
    {
        public readonly List<Error> Errors;
        public CustomException(List<Error> _errors) : base(string.Join(",", _errors.Select(er => er.ToString())))
        {
            Errors = _errors;
        }

        public override string ToString()
        {
            if (Errors.Count > 0)
            {
                return $"{base.ToString()}";
            }
            else
            {
                return $"CustomException: No Errors Associated";
            }
            
        }
    }
}
