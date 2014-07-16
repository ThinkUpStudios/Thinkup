namespace ThinkUp.Sdk.Contracts.ServerMessages
{
    public class TypingChatReceivedServerMessage : IServerMessage
    {
        public string FromUserName { get; set; }

        public string Message { get; set; }
    }
}
