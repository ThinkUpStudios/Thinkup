namespace ThinkUp.Sdk.Contracts.ServerMessages
{
    public class UserDisconnectedServerMessage : IServerMessage
    {
        public string Message
        {
            get
            {
                return string.Format("{0} is has been disconnected", this.UserName);
            }
        }

        public string UserName { get; set; }
    }
}
