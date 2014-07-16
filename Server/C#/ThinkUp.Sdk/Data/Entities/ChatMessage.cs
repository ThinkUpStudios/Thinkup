using ThinkUp.Sdk.Interfaces;

namespace ThinkUp.Sdk.Data.Entities
{
    public class ChatMessage : DataEntity, IChatMessage
    {
        public string From { get; set; }

        public string To { get; set; }

        public string Message { get; set; }
    }
}
