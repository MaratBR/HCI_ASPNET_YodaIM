using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YodaIM.Helpers.Errors
{
    public interface IError
    {
        string Description { get; }
        string Details { get; }
        object Extra { get; }
        IEnumerable<IError> Errors { get; }
    }
}
