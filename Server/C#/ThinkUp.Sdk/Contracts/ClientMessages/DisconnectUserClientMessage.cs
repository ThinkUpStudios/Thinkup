namespace ThinkUp.Sdk.Contracts.ClientMessages
{
    public class DisconnectUserClientMessage : IClientMessage
    {
        public string UserName { get; set; }
    }
}
