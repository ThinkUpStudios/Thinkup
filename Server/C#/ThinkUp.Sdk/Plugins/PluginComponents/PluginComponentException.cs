using System;

namespace ThinkUp.Sdk.Plugins.PluginComponents
{
    public class PluginComponentException: ApplicationException
    {
        public PluginComponentException()
        {
        }

        public PluginComponentException(string message)
            : base(message)
        {
        }

        public PluginComponentException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
