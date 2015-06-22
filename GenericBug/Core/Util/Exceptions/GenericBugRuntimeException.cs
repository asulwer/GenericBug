using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericBug.Core.Util.Exceptions
{
    [Serializable]
    public class GenericBugRuntimeException : GenericBugException
    {
        public GenericBugRuntimeException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public GenericBugRuntimeException(string message)
            : base(message)
        {
        }
    }
}
