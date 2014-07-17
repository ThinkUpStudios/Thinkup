namespace ThinkUp.Sdk.Contracts.ServerMessages
{
    public class ChatReceivedServerMessage : IServerMessage
    {
        public string ConversationName { get; set; }

        public string FromUserName { get; set; }

        public string Message { get; set; }
    }
}
