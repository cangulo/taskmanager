using System;
using System.Collections.Generic;
using System.Linq;
using TaskManagerAPI.Models.Errors;
using TaskManagerAPI.Models.Exceptions;

namespace TaskManagerAPI.Repositories.Exceptions
{
    public class RepositoryException : Exception, ICustomException
    {
        private readonly IEnumerable<CustomError> _errors;
        public RepositoryException(List<CustomError> errors) : base(string.Join(",", errors.Select(er => er.ToString())))
        {
            _errors = errors;
        }

        public override string ToString()
        {
            if (_errors.Any())
            {
                return $"{base.ToString()}";
            }
            else
            {
                return $"RepositoryException: No Errors Associated";
            }

        }

        public IEnumerable<CustomError> Errors()
        {
            return this._errors;
        }
    }
}
