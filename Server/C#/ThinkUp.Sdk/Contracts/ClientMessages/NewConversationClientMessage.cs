using System.Collections.Generic;

namespace ThinkUp.Sdk.Contracts.ClientMessages
{
    public class NewConversationClientMessage : IClientMessage
    {
        public string UserName { get; set; }

        public string ConversationName { get; set; }

        public IEnumerable<string> Participants { get; set; }
    }
}
