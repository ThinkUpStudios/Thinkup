namespace ThinkUp.Sdk.Contracts
{
    public class ContractMessage
    {
        public int Type { get; set; }

        public string Sender { get; set; }

        public string SerializedMessageObject { get; set; }

        public ContractMessage(int type)
        {
            this.Type = type;
        }

        public ContractMessage()
        {
        }
    }
}
