using System;
using System.Runtime.Serialization;

namespace GenericBug.Core.Util.Exceptions
{
    [Serializable]
    public class GenericBugException : Exception
    {
        public GenericBugException()
        {
        }

        public GenericBugException(string message)
            : base(message)
        {
        }

        public GenericBugException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected GenericBugException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
