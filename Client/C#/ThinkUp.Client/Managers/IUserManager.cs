using System;
using ThinkUp.Sdk.Contracts.ClientMessages;
using ThinkUp.Sdk.Contracts.ServerMessages;

namespace ThinkUp.Client.SignalR.Managers
{
	public interface IUserManager
	{
		event EventHandler<ServerMessageEventArgs<UserConnectedServerMessage>> UserConnectedNotificationReceived;
		
		event EventHandler<ServerMessageEventArgs<UserDisconnectedServerMessage>> UserDisconnectedNotificationReceived;
		
		event EventHandler<ServerMessageEventArgs<ConnectedUsersListServerMessage>> ConnectedUsersListNotificationReceived;

		void DisconnectUser(DisconnectUserClientMessage disconnectUserClientMessage);

		void RequestConnectedUsers(GetConnectedUsersClientMessage getConnectedUsersClientMessage);
	}
}
