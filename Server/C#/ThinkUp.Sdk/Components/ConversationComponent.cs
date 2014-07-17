using ThinkUp.Sdk.Contracts.ClientMessages;
using ThinkUp.Sdk.Contracts.ServerMessages;
using ThinkUp.Sdk.Services;

namespace ThinkUp.Sdk.Components
{
    public class ConversationComponent : ComponentBase
    {
        private readonly IConversationService conversationService;
        private readonly ISerializer serializer;

        public ConversationComponent(IConversationService conversationService, INotificationService notificationService, ISerializer serializer)
            : base(notificationService)
        {
            this.conversationService = conversationService;
            this.serializer = serializer;
        }

        public override bool CanHandleClientMessage(ClientContract clientContract)
        {
            return clientContract.Type == ClientMessageType.Chat || 
                clientContract.Type == ClientMessageType.TypingChat;
        }

        public override bool CanHandleServerMessage(ServerContract serverContract)
        {
            return serverContract.Type == ServerMessageType.ChatReceived || 
                serverContract.Type == ServerMessageType.TypingChatReceived;
        }

        public override void HandleClientMessage(ClientContract clientContract)
        {
            if (clientContract.Type == ClientMessageType.Chat)
            {
                var chatClientMessage = this.serializer.Deserialize<ChatClientMessage>(clientContract.SerializedClientMessage);
                var notification = new ChatReceivedServerMessage
                {
                    FromUserName = chatClientMessage.UserName,
                    Message = chatClientMessage.Message
                };

                this.notificationService.Send(ServerMessageType.ChatReceived, notification, chatClientMessage.ToUserName);
            }
            else if (clientContract.Type == ClientMessageType.TypingChat)
            {
                var typingChatClientMessage = this.serializer.Deserialize<TypingChatClientMessage>(clientContract.SerializedClientMessage);
                var notification = new TypingChatReceivedServerMessage
                {
                    FromUserName = typingChatClientMessage.UserName,
                    Message = typingChatClientMessage.Message
                };

                this.notificationService.Send(ServerMessageType.TypingChatReceived, notification, typingChatClientMessage.ToUserName);
            }
        }
    }
}
