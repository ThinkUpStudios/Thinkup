namespace ThinkUp.Sdk.Contracts.ClientMessages
{
    public class ChatClientMessage : IClientMessage
    {
        public string ConversationName { get; set; }

        public string UserName { get; set; }

        public string Message { get; set; }
    }
}
