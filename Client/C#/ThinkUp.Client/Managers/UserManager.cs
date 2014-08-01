using System;
using ThinkUp.Client.SignalR.Client;
using ThinkUp.Client.SignalR.Services;
using ThinkUp.Sdk.Contracts.ClientMessages;
using ThinkUp.Sdk.Contracts.ServerMessages;

namespace ThinkUp.Client.SignalR.Managers
{
	public class UserManager
	{
		private readonly IServerListener<UserConnectedServerMessage> connectedUserListener;
		private readonly IPluginService<DisconnectUserClientMessage, UserDisconnectedServerMessage> disconnectUserService;
		private readonly IPluginService<GetConnectedUsersClientMessage, ConnectedUsersListServerMessage> connectedUsersService;

		public event EventHandler<ServerMessageEventArgs<UserConnectedServerMessage>> UserConnectedNotificationReceived;
		public event EventHandler<ServerMessageEventArgs<UserDisconnectedServerMessage>> UserDisconnectedNotificationReceived;
		public event EventHandler<ServerMessageEventArgs<ConnectedUsersListServerMessage>> ConnectedUsersListNotificationReceived;

		public UserManager(IPluginClient pluginClient, ISerializer serializer)
        {
			this.connectedUserListener = new ServerListener<UserConnectedServerMessage>(ServerMessageType.UserConnected, pluginClient, serializer);
			this.disconnectUserService = new PluginService<DisconnectUserClientMessage, UserDisconnectedServerMessage>(ClientMessageType.DisconnectUser, ServerMessageType.UserDisconnected, pluginClient, serializer);
			this.connectedUsersService = new PluginService<GetConnectedUsersClientMessage, ConnectedUsersListServerMessage>(ClientMessageType.GetConnectedUsers, ServerMessageType.ConnectedUsersList, pluginClient, serializer);

            this.connectedUserListener.NotificationReceived += (sender, args) =>
            {
				if (this.UserConnectedNotificationReceived != null)
				{
					this.UserConnectedNotificationReceived(this, args);
				}
            };

            this.disconnectUserService.NotificationReceived += (sender, args) =>
            {
				if (this.UserDisconnectedNotificationReceived != null)
				{
					this.UserDisconnectedNotificationReceived(this, args);
				}
            };

            this.connectedUsersService.NotificationReceived += (sender, args) =>
            {
				if (this.ConnectedUsersListNotificationReceived != null)
				{
					this.ConnectedUsersListNotificationReceived(this, args);
				}
            };
        }

		public void DisconnectUser(DisconnectUserClientMessage disconnectUserClientMessage)
        {
			this.disconnectUserService.Send(disconnectUserClientMessage);
        }

		public void GetConnectedUsers(GetConnectedUsersClientMessage getConnectedUsersClientMessage)
        {
			this.connectedUsersService.Send(getConnectedUsersClientMessage);
        }
	}
}
