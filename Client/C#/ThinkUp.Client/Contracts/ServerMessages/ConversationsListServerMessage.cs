using System.Collections.Generic;

namespace ThinkUp.Sdk.Contracts.ServerMessages
{
    public class ConversationsListServerMessage : IServerMessage
    {
        public string Message
        {
            get
            {
                return string.Format("There is a total of {0} conversations", this.ConversationsCount);
            }
        }

        public IEnumerable<string> ConversationNames { get; set; }

        public int ConversationsCount { get; set; }
    }
}
