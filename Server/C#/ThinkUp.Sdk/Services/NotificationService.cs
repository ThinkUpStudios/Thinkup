using System;
using ThinkUp.Sdk.Contracts;

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
            var notification = new ContractMessage
            {
                Type = notificationType,
                SerializedMessageObject = this.serializer.Serialize(serverMessage)
            };

            this.Send(notification, userName);
        }

        public void Send(ContractMessage notification, string userName)
        {
            var sendMessageHandler = this.Notification;

            if (sendMessageHandler != null)
            {
                sendMessageHandler(this, new NotificationEventArgs(userName, notification));
            }
        }
    }
}
