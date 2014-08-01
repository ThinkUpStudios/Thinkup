using System;

namespace ThinkUp.Sdk.Contracts.ClientMessages
{
    public class GetConversationClientMessage : IClientMessage
    {
        public string UserName { get; set; }

        public string ConversationName { get; set; }
    }
}
