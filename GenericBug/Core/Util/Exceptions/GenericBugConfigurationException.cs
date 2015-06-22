using System;
using System.Linq.Expressions;

namespace GenericBug.Core.Util.Exceptions
{
    [Serializable]
    public class GenericBugConfigurationException : GenericBugException
    {
        public GenericBugConfigurationException(string message, Exception inner) : base(message, inner)
        {
            this.MisconfiguredProperty = string.Empty;
        }

        private GenericBugConfigurationException(string propertyName, string message) : base(message)
        {
            this.MisconfiguredProperty = propertyName;
        }

        public string MisconfiguredProperty { get; set; }

        public static GenericBugConfigurationException Create<T>(Expression<Func<T>> propertyExpression, string message)
        {
            return new GenericBugConfigurationException(((MemberExpression)propertyExpression.Body).Member.Name, message);
        }
    }
}
