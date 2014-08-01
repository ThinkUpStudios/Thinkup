namespace ThinkUp.Sdk.Contracts.ClientMessages
{
    public class ClientContract
    {
        public int Type { get; set; }

        public string Sender { get; set; }

        public string SerializedClientMessage { get; set; }

        public ClientContract(int type)
        {
            this.Type = type;
        }

        public ClientContract()
        {
        }
    }
}
