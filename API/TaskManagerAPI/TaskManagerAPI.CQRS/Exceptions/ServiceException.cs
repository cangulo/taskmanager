using System;
using System.Collections.Generic;
using System.Linq;
using TaskManagerAPI.Models.Errors;
using TaskManagerAPI.Models.Exceptions;

namespace TaskManagerAPI.CQRS.Exceptions
{
    public class ServiceException : Exception, ICustomException
    {
        private readonly List<ErrorCodeAndMessage> _errors;
        public ServiceException(List<ErrorCodeAndMessage> errors) : base(string.Join(",", errors.Select(er => er.ToString())))
        {
            _errors = errors;
        }

        public override string ToString()
        {
            if (_errors.Count > 0)
            {
                return $"{base.ToString()}";
            }
            else
            {
                return $"ServiceException: No Errors Associated";
            }

        }

        public List<ErrorCodeAndMessage> Errors()
        {
            return this._errors;
        }
    }
}
