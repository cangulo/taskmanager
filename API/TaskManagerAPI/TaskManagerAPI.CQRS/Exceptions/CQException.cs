using System;
using System.Collections.Generic;
using System.Linq;
using TaskManagerAPI.Models.Errors;
using TaskManagerAPI.Models.Exceptions;

namespace TaskManagerAPI.CQRS.Exceptions
{
    public class CQException : Exception, ICustomException
    {
        private readonly IEnumerable<ErrorCodeAndMessage> _errors;
        public CQException(IEnumerable<ErrorCodeAndMessage> errors) : base(string.Join(",", errors.Select(er => er.ToString())))
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
                return $"ServiceException: No Errors Associated";
            }

        }

        public IEnumerable<ErrorCodeAndMessage> Errors()
        {
            return this._errors;
        }
    }
}
