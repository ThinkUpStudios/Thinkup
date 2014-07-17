namespace ThinkUp.Sdk.Contracts.ClientMessages
{
    public class ClientMessageType
    {
        public const int ConnectUser = 100;
        public const int GetConnectedUsers = 101;
        public const int DisconnectUser = 102;
        public const int NewConversation = 103;
        public const int NewConversationParticipant = 104;
        public const int GetConversations = 105;
        public const int GetConversation = 106;
        public const int Chat = 107;
        public const int TypingChat = 108;
        public const int LeaveConversation = 109;
    }
}
