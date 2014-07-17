using System;
using ThinkUp.Sdk.Contracts;
using ThinkUp.Sdk.Contracts.ServerMessages;

namespace ThinkUp.Sdk.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ISerializer serializer;

        public event EventHandler<NotificationEventArgs> Notification;

        public NotificationService(ISerializer serializer)
        {
            this.serializer = serializer;
        }

        public void SendBroadcast(int notificationType, object serverMessage, params string[] userNames)
        {
            foreach (var userName in userNames)
            {
                this.Send(notificationType, serverMessage, userName);
            }
        }

        public void Send(int notificationType, object serverMessage, string userName)
        {
            var notification = new ServerContract
            {
                Type = notificationType,
                SerializedServerMessage = this.serializer.Serialize(serverMessage)
            };

            this.Send(notification, userName);
        }

        public void Send(ServerContract serverContract, string userName)
        {
            var sendMessageHandler = this.Notification;

            if (sendMessageHandler != null)
            {
                sendMessageHandler(this, new NotificationEventArgs(userName, serverContract));
            }
        }
    }
}
