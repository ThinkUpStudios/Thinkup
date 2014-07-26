using System;

namespace ThinkUp.Sdk.Setup
{
    public class SetupException: ApplicationException
    {
        public SetupException()
        {
        }

        public SetupException(string message)
            : base(message)
        {
        }

        public SetupException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
