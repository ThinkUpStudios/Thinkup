using System;
using ThinkUp.Sdk.Contracts.ServerMessages;

namespace ThinkUp.Sdk
{
    public class NotificationEventArgs : EventArgs
    {
        public string Receiver { get; set; }

        public ServerContract Notification { get; set; }

        public NotificationEventArgs(string receiver, ServerContract serverContract)
        {
            this.Receiver = receiver;
            this.Notification = serverContract;
        }

        public NotificationEventArgs()
        {
        }
    }
}
