using System;

namespace ThinkUp.Sdk.Data
{
    public class DataException : ApplicationException
    {
        public DataException()
        {
        }

        public DataException(string message)
            : base(message)
        {
        }

        public DataException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
