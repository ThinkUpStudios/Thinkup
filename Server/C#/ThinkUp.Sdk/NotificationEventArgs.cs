using System;
using ThinkUp.Sdk.Contracts;

namespace ThinkUp.Sdk
{
    public class NotificationEventArgs : EventArgs
    {
        public string Receiver { get; set; }

        public ContractMessage Notification { get; set; }

        public NotificationEventArgs(string receiver, ContractMessage notification)
        {
            this.Receiver = receiver;
            this.Notification = notification;
        }

        public NotificationEventArgs()
        {
        }
    }
}
