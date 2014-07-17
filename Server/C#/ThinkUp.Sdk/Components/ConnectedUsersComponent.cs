using System.Linq;
using ThinkUp.Sdk.Contracts.ClientMessages;
using ThinkUp.Sdk.Contracts.ServerMessages;
using ThinkUp.Sdk.Services;

namespace ThinkUp.Sdk.Components
{ 
    public class ConnectedUsersComponent : ComponentBase
    {
        private readonly IUserService userService;
        private readonly ISerializer serializer;

        public ConnectedUsersComponent(IUserService userService, INotificationService notificationService, ISerializer serializer)
            : base(notificationService)
        {
            this.userService = userService;
            this.serializer = serializer;
        }

        public override bool CanHandleClientMessage(ClientContract clientContract)
        {
            return clientContract.Type == ClientMessageType.GetConnectedUsers;
        }

        public override bool CanHandleServerMessage(ServerContract serverContract)
        {
            return serverContract.Type == ServerMessageType.ConnectedUsersList;
        }

        public override void HandleClientMessage(ClientContract clientContract)
        {
            var getConnectedUsersClientMessage = this.serializer.Deserialize<GetConnectedUsersClientMessage>(clientContract.SerializedClientMessage);
            var sortedUsers = this.userService.GetAllConnected(userNameToExclude: getConnectedUsersClientMessage.UserName)
                .OrderBy(p => p.Name);
            var sortedUsersPage = sortedUsers.Take(getConnectedUsersClientMessage.PageSize);
            var notification = new ConnectedUsersListServerMessage
            {
                UserName = getConnectedUsersClientMessage.UserName,
                ConnectedUserNames = sortedUsersPage.Select(p => p.Name),
                ConectedUsersCount = sortedUsers.Count()
            };

            this.notificationService.Send(ServerMessageType.ConnectedUsersList, notification, getConnectedUsersClientMessage.UserName);
        }
    }
}
