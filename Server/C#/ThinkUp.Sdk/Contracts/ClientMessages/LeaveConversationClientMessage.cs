namespace ThinkUp.Sdk.Contracts.ClientMessages
{
    public class LeaveConversationClientMessage : IClientMessage
    {
        public string ConversationName { get; set; }

        public string UserName { get; set; }
    }
}
