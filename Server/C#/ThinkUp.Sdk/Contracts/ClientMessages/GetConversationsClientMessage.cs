namespace ThinkUp.Sdk.Contracts.ClientMessages
{
    public class GetConversationsClientMessage : IClientMessage
    {
        public string UserName { get; set; }

        public int PageSize { get; set; }
    }
}
