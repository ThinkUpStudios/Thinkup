namespace ThinkUp.Sdk.Contracts.ClientMessages
{
    public class NewConversationParticipantClientMessage : IClientMessage
    {
        public string UserName { get; set; }

        public string ConversationName { get; set; }

        public string NewParticipantName { get; set; }
    }
}
