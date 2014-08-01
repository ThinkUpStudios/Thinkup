using System;
using ThinkUp.Client.SignalR.Client;
using ThinkUp.Client.SignalR.Services;
using ThinkUp.Sdk.Contracts.ClientMessages;
using ThinkUp.Sdk.Contracts.ServerMessages;

namespace ThinkUp.Client.SignalR.Managers
{
	public class ConversationManager : IConversationManager
	{
		private IServerSender<NewConversationClientMessage> newConversationSender;
		private IServerSender<NewConversationParticipantClientMessage> newParticipantSender;
		private IPluginService<LeaveConversationClientMessage, ConversationLeftServerMessage> leaveConversationService;
		private IPluginService<GetConversationsClientMessage, ConversationsListServerMessage> conversationsService;
		private IPluginService<GetConversationClientMessage, ConversationDetailServerMessage> conversationDetailService;
		private IPluginService<ChatClientMessage, ChatReceivedServerMessage> messageService;
		private IPluginService<TypingChatClientMessage, TypingChatReceivedServerMessage> typingIndicationService;

		public event EventHandler<ServerMessageEventArgs<ConversationLeftServerMessage>> ConversationLeftNotificationReceived;

		public event EventHandler<ServerMessageEventArgs<ConversationsListServerMessage>> ConversationsListNotificationReceived;

		public event EventHandler<ServerMessageEventArgs<ConversationDetailServerMessage>> ConversationDetailNotificationReceived;

		public event EventHandler<ServerMessageEventArgs<ChatReceivedServerMessage>> NewMessageNotificationReceived;

		public event EventHandler<ServerMessageEventArgs<TypingChatReceivedServerMessage>> TypingIndicatorNotificationReceived;

		public ConversationManager(IPluginClient pluginClient, ISerializer serializer)
		{
			this.newConversationSender = new ServerSender<NewConversationClientMessage>(ClientMessageType.NewConversation, pluginClient, serializer);
			this.newParticipantSender = new ServerSender<NewConversationParticipantClientMessage>(ClientMessageType.NewConversationParticipant, pluginClient, serializer);
			this.leaveConversationService = new PluginService<LeaveConversationClientMessage, ConversationLeftServerMessage>(ClientMessageType.LeaveConversation, ServerMessageType.ConversationLeft, pluginClient, serializer);
			this.conversationsService = new PluginService<GetConversationsClientMessage, ConversationsListServerMessage>(ClientMessageType.GetConversations, ServerMessageType.ConversationsList, pluginClient, serializer);
			this.conversationDetailService = new PluginService<GetConversationClientMessage, ConversationDetailServerMessage>(ClientMessageType.GetConversation, ServerMessageType.ConversationDetail, pluginClient, serializer);
			this.messageService = new PluginService<ChatClientMessage, ChatReceivedServerMessage>(ClientMessageType.Chat, ServerMessageType.ChatReceived, pluginClient, serializer);
			this.typingIndicationService = new PluginService<TypingChatClientMessage, TypingChatReceivedServerMessage>(ClientMessageType.TypingChat, ServerMessageType.TypingChatReceived, pluginClient, serializer);

			this.leaveConversationService.NotificationReceived += (sender, args) =>
			{
				if (this.ConversationLeftNotificationReceived != null)
				{
					this.ConversationLeftNotificationReceived(this, args);
				}
			};

			this.conversationsService.NotificationReceived += (sender, args) =>
			{
				if (this.ConversationsListNotificationReceived != null)
				{
					this.ConversationsListNotificationReceived(this, args);
				}
			};

			this.conversationDetailService.NotificationReceived += (sender, args) =>
			{
				if (this.ConversationDetailNotificationReceived != null)
				{
					this.ConversationDetailNotificationReceived(this, args);
				}
			};

			this.messageService.NotificationReceived += (sender, args) =>
			{
				if (this.NewMessageNotificationReceived != null)
				{
					this.NewMessageNotificationReceived(this, args);
				}
			};

			this.typingIndicationService.NotificationReceived += (sender, args) =>
			{
				if (this.TypingIndicatorNotificationReceived != null)
				{
					this.TypingIndicatorNotificationReceived(this, args);
				}
			};
		}

		public void CreateConversation(NewConversationClientMessage newConversationClientMessage)
		{
			this.newConversationSender.Send(newConversationClientMessage);
		}

		public void AddParticipant(NewConversationParticipantClientMessage newConversationParticipantClientMessage)
		{
			this.newParticipantSender.Send(newConversationParticipantClientMessage);
		}

		public void LeaveConversation(LeaveConversationClientMessage leaveConversationClientMessage)
		{
			this.leaveConversationService.Send(leaveConversationClientMessage);
		}

		public void RequestConversations(GetConversationsClientMessage getConversationsClientMessage)
		{
			this.conversationsService.Send(getConversationsClientMessage);
		}

		public void RequestConversation(GetConversationClientMessage getConversationClientMessage)
		{
			this.conversationDetailService.Send(getConversationClientMessage);
		}

		public void SendMessage(ChatClientMessage chatClientMessage)
		{
			this.messageService.Send(chatClientMessage);
		}

		public void SendTypingIndicator(TypingChatClientMessage typingChatClientMessage)
		{
			this.typingIndicationService.Send(typingChatClientMessage);
		}
	}
}
