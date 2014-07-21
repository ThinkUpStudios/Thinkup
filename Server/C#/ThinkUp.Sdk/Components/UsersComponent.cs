using System.Linq;
using ThinkUp.Sdk.Contracts.ClientMessages;
using ThinkUp.Sdk.Contracts.ServerMessages;
using ThinkUp.Sdk.Services;

namespace ThinkUp.Sdk.Components
{
    public class UsersComponent : ComponentBase
    {
        private readonly IUserService userService;

        public UsersComponent(IUserService userService, INotificationService notificationService, ISerializer serializer)
            : base(notificationService, serializer)
        {
            this.userService = userService;
        }

        public override bool CanHandleClientMessage(ClientContract clientContract)
        {
            return clientContract.Type == ClientMessageType.GetConnectedUsers || 
                clientContract.Type == ClientMessageType.ConnectUser ||
                clientContract.Type == ClientMessageType.DisconnectUser;
        }

        public override bool CanHandleServerMessage(ServerContract serverContract)
        {
            return serverContract.Type == ServerMessageType.ConnectedUsersList || 
                serverContract.Type == ServerMessageType.UserConnected ||
                serverContract.Type == ServerMessageType.UserDisconnected;
        }

        public override void HandleClientMessage(ClientContract clientContract)
        {
            switch (clientContract.Type)
            {
                case ClientMessageType.GetConnectedUsers:
                    this.HandleGetConnectedUsers(clientContract);
                    break;
                case ClientMessageType.ConnectUser:
                    this.HandleConnectUser(clientContract);
                    break;
                case ClientMessageType.DisconnectUser:
                    this.HandleDisconnectUser(clientContract);
                    break;
            }
        }

        private void HandleGetConnectedUsers(ClientContract clientContract)
        {
            var getConnectedUsersClientMessage = this.serializer.Deserialize<GetConnectedUsersClientMessage>(clientContract.SerializedClientMessage);
            var connectedUsers = this.userService.GetAllConnected(userNameToExclude: getConnectedUsersClientMessage.UserName)
                .OrderBy(p => p.Name);
            var connectedUsersPage = connectedUsers.Take(getConnectedUsersClientMessage.PageSize);
            var connectedUsersListServerMessage = new ConnectedUsersListServerMessage
            {
                UserName = getConnectedUsersClientMessage.UserName,
                ConnectedUserNames = connectedUsersPage.Select(p => p.Name),
                ConectedUsersCount = connectedUsers.Count()
            };

            this.notificationService.Send(ServerMessageType.ConnectedUsersList, connectedUsersListServerMessage, getConnectedUsersClientMessage.UserName);
        }

        private void HandleConnectUser(ClientContract clientContract)
        {
            var connectUserClientMessage = this.serializer.Deserialize<ConnectUserClientMessage>(clientContract.SerializedClientMessage);

            this.userService.Connect(connectUserClientMessage.UserName);

            var userConnectedServerMessage = new UserConnectedServerMessage
            {
                UserName = connectUserClientMessage.UserName
            };
            var usersToNotify = this.userService.GetAllConnected(userNameToExclude: connectUserClientMessage.UserName)
                .Select(p => p.Name);

            this.notificationService.SendBroadcast(ServerMessageType.UserConnected, userConnectedServerMessage, usersToNotify.ToArray());
        }

        private void HandleDisconnectUser(ClientContract clientContract)
        {
            var disconnectUserClientMessage = this.serializer.Deserialize<DisconnectUserClientMessage>(clientContract.SerializedClientMessage);

            this.userService.Disconnect(disconnectUserClientMessage.UserName);

            var userDisconnectedServerMessage = new UserDisconnectedServerMessage
            {
                UserName = disconnectUserClientMessage.UserName
            };
            var usersToNotify = this.userService.GetAllConnected(userNameToExclude: disconnectUserClientMessage.UserName)
                .Select(p => p.Name);

            this.notificationService.SendBroadcast(ServerMessageType.UserDisconnected, userDisconnectedServerMessage, usersToNotify.ToArray());
        }
    }
}
