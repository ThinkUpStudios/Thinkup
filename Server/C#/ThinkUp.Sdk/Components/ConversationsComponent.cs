using System.Collections.Generic;
using System.Linq;
using ThinkUp.Sdk.Contracts.ClientMessages;
using ThinkUp.Sdk.Contracts.ServerMessages;
using ThinkUp.Sdk.Services;

namespace ThinkUp.Sdk.Components
{
    public class ConversationsComponent : ComponentBase
    {
        private readonly IConversationService conversationService;
        private readonly ISerializer serializer;

        public ConversationsComponent(IConversationService conversationService, INotificationService notificationService, ISerializer serializer)
            : base(notificationService)
        {
            this.conversationService = conversationService;
            this.serializer = serializer;
        }

        public override bool CanHandleClientMessage(ClientContract clientContract)
        {
            return clientContract.Type == ClientMessageType.NewConversation ||
                clientContract.Type == ClientMessageType.NewConversationParticipant || 
                clientContract.Type == ClientMessageType.GetConversations ||
                clientContract.Type == ClientMessageType.GetConversation ||
                clientContract.Type == ClientMessageType.Chat ||
                clientContract.Type == ClientMessageType.TypingChat ||
                clientContract.Type == ClientMessageType.LeaveConversation;
        }

        public override bool CanHandleServerMessage(ServerContract serverContract)
        {
            return serverContract.Type == ServerMessageType.ConversationsList ||
                serverContract.Type == ServerMessageType.ConversationDetail ||
                serverContract.Type == ServerMessageType.ChatReceived || 
                serverContract.Type == ServerMessageType.TypingChatReceived ||
                serverContract.Type == ServerMessageType.ConversationLeft; 
        }

        public override void HandleClientMessage(ClientContract clientContract)
        {
            switch (clientContract.Type)
            {
                case ClientMessageType.NewConversation:
                    this.HandleNewConversation(clientContract);
                    break;
                case ClientMessageType.NewConversationParticipant:
                    this.HandleNewConversationParticipant(clientContract);
                    break;
                case ClientMessageType.GetConversations:
                    this.HandleGetConversations(clientContract);
                    break;
                case ClientMessageType.GetConversation:
                    this.HandleGetConversation(clientContract);
                    break;
                case ClientMessageType.Chat:
                    this.HandleChat(clientContract);
                    break;
                case ClientMessageType.TypingChat:
                    this.HandleTypingChat(clientContract);
                    break;
                case ClientMessageType.LeaveConversation:
                    this.HandleLeaveConversation(clientContract);
                    break;
            }
        }

        private void HandleNewConversation(ClientContract clientContract)
        {
            var newConversationClientMessage = this.serializer.Deserialize<NewConversationClientMessage>(clientContract.SerializedClientMessage);
            var participants = new List<string>();

            if (!newConversationClientMessage.Participants.Contains(newConversationClientMessage.UserName))
            {
                participants.Add(newConversationClientMessage.UserName);
            }

            participants.AddRange(newConversationClientMessage.Participants);

            if (string.IsNullOrEmpty(newConversationClientMessage.ConversationName))
            {
                this.conversationService.CreateConversation(participants.ToArray());
            }
            else
            {
                this.conversationService.CreateConversation(newConversationClientMessage.ConversationName, participants.ToArray());
            }
        }

        private void HandleNewConversationParticipant(ClientContract clientContract)
        {
            var newConversationParticipantClientMessage = this.serializer.Deserialize<NewConversationParticipantClientMessage>(clientContract.SerializedClientMessage);

            this.conversationService.AddParticipant(newConversationParticipantClientMessage.ConversationName, newConversationParticipantClientMessage.NewParticipantName);
        }

        private void HandleGetConversations(ClientContract clientContract)
        {
            var getConversationsClientMessage = this.serializer.Deserialize<GetConversationsClientMessage>(clientContract.SerializedClientMessage);
            var conversations = this.conversationService.GetAllByParticipant(getConversationsClientMessage.UserName);
            var conversationsPage = conversations.Take(getConversationsClientMessage.PageSize);
            var conversationsListServerMessage = new ConversationsListServerMessage
            {
                ConversationNames = conversationsPage.Select(c => c.Name),
                ConversationsCount = conversationsPage.Count()
            };

            this.notificationService.Send(ServerMessageType.ConversationsList, conversationsListServerMessage, getConversationsClientMessage.UserName);
        }

        private void HandleGetConversation(ClientContract clientContract)
        {
            var getConversationClientMessage = this.serializer.Deserialize<GetConversationClientMessage>(clientContract.SerializedClientMessage);
            var conversation = this.conversationService.GetByName(getConversationClientMessage.ConversationName);
            var conversationDetailServerMessage = new ConversationDetailServerMessage
            {
                ConversationName = conversation.Name,
            };

            foreach (var participant in conversation.GetParticipants())
            {
                conversationDetailServerMessage.AddParticipant(participant);
            }

            foreach (var message in conversation.GetMessages())
            {
                conversationDetailServerMessage.AddMessage(new ChatMessage
                {
                    Sender = message.Sender,
                    Content = message.Content,
                    Date = message.Date
                });
            }

            this.notificationService.Send(ServerMessageType.ConversationDetail, conversationDetailServerMessage, getConversationClientMessage.UserName);
        }

        private void HandleChat(ClientContract clientContract)
        {
            var chatClientMessage = this.serializer.Deserialize<ChatClientMessage>(clientContract.SerializedClientMessage);

            this.conversationService.AddMessage(chatClientMessage.ConversationName, chatClientMessage.UserName, chatClientMessage.Message);

            var chatReceivedServerMessage = new ChatReceivedServerMessage
            {
                ConversationName = chatClientMessage.ConversationName,
                FromUserName = chatClientMessage.UserName,
                Message = chatClientMessage.Message
            };
            var usersToNotify = this.conversationService.GetByName(chatClientMessage.ConversationName)
                .GetParticipants().Where(p => p != chatClientMessage.UserName);

            this.notificationService.SendBroadcast(ServerMessageType.ChatReceived, chatReceivedServerMessage, usersToNotify.ToArray());
        }

        private void HandleTypingChat(ClientContract clientContract)
        {
            var typingChatClientMessage = this.serializer.Deserialize<TypingChatClientMessage>(clientContract.SerializedClientMessage);
            var typingChatReceivedServerMessage = new TypingChatReceivedServerMessage
            {
                ConversationName = typingChatClientMessage.ConversationName,
                FromUserName = typingChatClientMessage.UserName,
                Message = typingChatClientMessage.Message
            };
            var usersToNotify = this.conversationService.GetByName(typingChatClientMessage.ConversationName)
                .GetParticipants().Where(p => p != typingChatClientMessage.UserName);

            this.notificationService.SendBroadcast(ServerMessageType.TypingChatReceived, typingChatReceivedServerMessage, usersToNotify.ToArray());
        }

        private void HandleLeaveConversation(ClientContract clientContract)
        {
            var leaveConversationClientMessage = this.serializer.Deserialize<LeaveConversationClientMessage>(clientContract.SerializedClientMessage);
            var conversation = this.conversationService.GetByName(leaveConversationClientMessage.ConversationName);
            var participants = conversation.GetParticipants().Where(p => p != leaveConversationClientMessage.UserName);

            if (participants.Count() <= 1)
            {
                this.conversationService.DeleteConversation(leaveConversationClientMessage.ConversationName);
            }

            var conversationLeftServerMessage = new ConversationLeftServerMessage
            {
                ConversationName = leaveConversationClientMessage.ConversationName,
                UserName = leaveConversationClientMessage.UserName
            };

            this.notificationService.SendBroadcast(ServerMessageType.ConversationLeft, conversationLeftServerMessage, participants.ToArray());
        }
    }
}
