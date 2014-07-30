using ThinkUp.Sdk.Contracts.ServerMessages;

namespace ThinkUp.Sdk.Tests.TestModels
{
    public class TestServerMessage : IServerMessage
    {
        public string Message
        {
            get
            {
                return string.Format("This is a message for {0}", this.Name);
            }
        }

        public string Name { get; set; }
    }
}
