namespace ThinkUp.Sdk.Contracts.ServerMessages
{
    public class ServerContract
    {
        public int Type { get; set; }

        public string SerializedServerMessage { get; set; }

        public ServerContract(int type)
        {
            this.Type = type;
        }

        public ServerContract()
        {
        }
    }
}
