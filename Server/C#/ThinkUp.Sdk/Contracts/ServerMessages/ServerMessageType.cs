namespace ThinkUp.Sdk.Contracts.ServerMessages
{
    public class ServerMessageType
    {
        public const int UserConnected = 200;
        public const int ConnectedUsersList = 201;
        public const int UserDisconnected = 202;
        public const int ConversationsList = 203;
        public const int ConversationDetail = 204;
        public const int ChatReceived = 205;
        public const int TypingChatReceived = 206;
        public const int ConversationLeft = 207;
        public const int Error = 208;
    }
}
