using System;
using ThinkUp.Sdk.Contracts.ClientMessages;
using ThinkUp.Sdk.Contracts.ServerMessages;
using ThinkUp.Sdk.Services;

namespace ThinkUp.Sdk.Components
{
    public abstract class ComponentBase : IComponent
    {
        protected readonly ISerializer serializer;
        protected readonly INotificationService notificationService;

        public event EventHandler<ServerMessageEventArgs> ServerMessage;

        public string Name
        {
            get { return this.GetType().Name; }
        }

        protected ComponentBase(INotificationService notificationService, ISerializer serializer)
        {
            this.serializer = serializer;
            this.notificationService = notificationService;
            this.notificationService.Notification += (sender, e) =>
            {
                var contract = new ServerContract
                {
                    Type = e.NotificationType,
                    SerializedServerMessage = this.serializer.Serialize(e.Notification)
                };

                if (this.CanHandleServerMessage(contract))
                {
                    var serverMessageHandler = this.ServerMessage;

                    if (serverMessageHandler != null)
                    {
                        var serverMessageArgs = new ServerMessageEventArgs(contract, e.Receiver);

                        serverMessageHandler(this, serverMessageArgs);
                    }
                }
            };
        }

        public abstract bool CanHandleClientMessage(ClientContract clientContract);

        public abstract bool CanHandleServerMessage(ServerContract serverContract);

        ///<exception cref="ComponentException">ComponentException</exception>
        ///<exception cref="ServiceException">ServiceException</exception>
        public abstract void HandleClientMessage(ClientContract clientContract);
    }
}
