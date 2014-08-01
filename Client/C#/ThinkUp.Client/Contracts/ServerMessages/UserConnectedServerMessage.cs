namespace ThinkUp.Sdk.Contracts.ServerMessages
{
    public class UserConnectedServerMessage : IServerMessage
    {
        public string Message
        {
            get
            {
                return string.Format("{0} is now connected", this.UserName);
            }
        }

        public string UserName { get; set; }
    }
}
