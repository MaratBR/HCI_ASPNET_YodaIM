using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YodaIM.Helpers.Errors
{
    public class Error : IError
    {
        public Error(string message, string details = null, object extra = null, IEnumerable<IError> errors = null)
        {
            Errors = errors;
            Extra = extra;
            Details = details;
            Description = message;
        }

        public string Description { get; private set; }

        public string Details { get; private set; }

        public object Extra { get; private set; }

        public IEnumerable<IError> Errors { get; private set; }

    }
}
