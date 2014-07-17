using System.Linq;
using ThinkUp.Sdk.Contracts.ClientMessages;
using ThinkUp.Sdk.Contracts.ServerMessages;
using ThinkUp.Sdk.Services;

namespace ThinkUp.Sdk.Components
{
    public class DisconnectUserComponent : ComponentBase
    {
        private readonly IUserService userService;
        private readonly ISerializer serializer;

        public DisconnectUserComponent(IUserService userService, INotificationService notificationService,
            ISerializer serializer)
            : base(notificationService)
        {
            this.userService = userService;
            this.serializer = serializer;
        }

        public override bool CanHandleClientMessage(ClientContract clientContract)
        {
            return clientContract.Type == ClientMessageType.DisconnectUser;
        }

        public override bool CanHandleServerMessage(ServerContract serverContract)
        {
            return serverContract.Type == ServerMessageType.UserDisconnected;
        }

        public override void HandleClientMessage(ClientContract clientContract)
        {
            var disconnectUserClientMessage = this.serializer.Deserialize<DisconnectUserClientMessage>(clientContract.SerializedClientMessage);

            this.userService.Disconnect(disconnectUserClientMessage.UserName);

            var notification = new UserDisconnectedServerMessage
            {
                UserName = disconnectUserClientMessage.UserName
            };
            var usersToNotify = this.userService.GetAllConnected(userNameToExclude: disconnectUserClientMessage.UserName)
                .Select(p => p.Name);

            this.notificationService.SendBroadcast(ServerMessageType.UserDisconnected, notification, usersToNotify.ToArray());
        }
    }
}
