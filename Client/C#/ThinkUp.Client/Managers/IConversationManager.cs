using System;
using ThinkUp.Sdk.Contracts.ClientMessages;
using ThinkUp.Sdk.Contracts.ServerMessages;

namespace ThinkUp.Client.SignalR.Managers
{
	public interface IConversationManager
	{
		event EventHandler<ServerMessageEventArgs<ConversationLeftServerMessage>> ConversationLeftNotificationReceived;

		event EventHandler<ServerMessageEventArgs<ConversationsListServerMessage>> ConversationsListNotificationReceived;

		event EventHandler<ServerMessageEventArgs<ConversationDetailServerMessage>> ConversationDetailNotificationReceived;

		event EventHandler<ServerMessageEventArgs<ChatReceivedServerMessage>> NewMessageNotificationReceived;

		event EventHandler<ServerMessageEventArgs<TypingChatReceivedServerMessage>> TypingIndicatorNotificationReceived;

		void CreateConversation(NewConversationClientMessage newConversationClientMessage);

		void AddParticipant(NewConversationParticipantClientMessage newConversationParticipantClientMessage);

		void LeaveConversation(LeaveConversationClientMessage leaveConversationClientMessage);

		void RequestConversations(GetConversationsClientMessage getConversationsClientMessage);

		void RequestConversation(GetConversationClientMessage getConversationClientMessage);

		void SendMessage(ChatClientMessage chatClientMessage);

		void SendTypingIndicator(TypingChatClientMessage typingChatClientMessage);
	}
}
