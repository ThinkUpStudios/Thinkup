using System.Linq;
using ThinkUp.Sdk.Contracts.ClientMessages;
using ThinkUp.Sdk.Contracts.ServerMessages;
using ThinkUp.Sdk.Services;

namespace ThinkUp.Sdk.Components
{
    public class ConnectUserComponent : ComponentBase
    {
        private readonly IUserService userService;
        private readonly ISerializer serializer;

        public ConnectUserComponent(IUserService userService, INotificationService notificationService, ISerializer serializer)
            : base(notificationService)
        {
            this.userService = userService;
            this.serializer = serializer;
        }

        public override bool CanHandleClientMessage(ClientContract clientContract)
        {
            return clientContract.Type == ClientMessageType.ConnectUser;
        }

        public override bool CanHandleServerMessage(ServerContract serverContract)
        {
            return serverContract.Type == ServerMessageType.UserConnected;
        }

        public override void HandleClientMessage(ClientContract clientContract)
        {
            var connectUserClientMessage = this.serializer.Deserialize<ConnectUserClientMessage>(clientContract.SerializedClientMessage);

            this.userService.Connect(connectUserClientMessage.UserName);

            var notification = new UserConnectedServerMessage
            {
                UserName = connectUserClientMessage.UserName
            };
            var usersToNotify = this.userService.GetAllConnected(userNameToExclude: connectUserClientMessage.UserName)
                .Select(p => p.Name);

            this.notificationService.SendBroadcast(ServerMessageType.UserConnected, notification, usersToNotify.ToArray());
        }
    }
}
