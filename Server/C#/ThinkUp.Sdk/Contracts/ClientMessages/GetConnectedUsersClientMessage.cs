namespace ThinkUp.Sdk.Contracts.ClientMessages
{
    public class GetConnectedUsersClientMessage : IClientMessage
    {
        public string UserName { get; set; }

        public int PageSize { get; set; }
    }
}
