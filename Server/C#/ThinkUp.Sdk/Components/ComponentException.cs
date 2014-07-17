using System;

namespace ThinkUp.Sdk.Components
{
    public class ComponentException: ApplicationException
    {
        public ComponentException()
        {
        }

        public ComponentException(string message)
            : base(message)
        {
        }

        public ComponentException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
