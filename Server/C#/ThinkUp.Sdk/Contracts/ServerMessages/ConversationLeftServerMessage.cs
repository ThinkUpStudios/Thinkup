namespace ThinkUp.Sdk.Contracts.ServerMessages
{
    public class ConversationLeftServerMessage : IServerMessage
    {
        public string Message
        {
            get
            {
                return string.Format("{0} has left the conversation {1}", this.UserName, this.ConversationName);
            }
        }

        public string ConversationName { get; set; }

        public string UserName { get; set; }
    }
}
