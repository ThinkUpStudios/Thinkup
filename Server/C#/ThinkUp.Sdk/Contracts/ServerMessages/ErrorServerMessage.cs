namespace ThinkUp.Sdk.Contracts.ServerMessages
{
    public class ErrorServerMessage : IServerMessage
    {
        public int ErrorCode { get; set; }

        public string Message { get; set; }
    }
}
