using System;

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
            var notificationHandler = this.Notification;

            if (notificationHandler != null)
            {
                notificationHandler(this, new NotificationEventArgs(notificationType, serverMessage, userName));
            }
        }
    }
}
