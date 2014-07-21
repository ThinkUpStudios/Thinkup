using System;

namespace ThinkUp.Sdk
{
    public class NotificationEventArgs : EventArgs
    {
        public int NotificationType { get; set; }

        public object Notification { get; set; }

        public string Receiver { get; set; }

        public NotificationEventArgs(int notificationType, object notification, string receiver)
        {
            this.NotificationType = notificationType;
            this.Notification = notification;
            this.Receiver = receiver;
        }

        public NotificationEventArgs()
        {
        }
    }
}
