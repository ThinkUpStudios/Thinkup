using System;
using ThinkUp.Sdk.Contracts.ClientMessages;
using ThinkUp.Sdk.Contracts.ServerMessages;
using ThinkUp.Sdk.Services;

namespace ThinkUp.Sdk.Components
{
    public abstract class ComponentBase : IComponent
    {
        protected readonly INotificationService notificationService;

        public event EventHandler<NotificationEventArgs> Notification;

        public string Name
        {
            get { return this.GetType().Name; }
        }

        protected ComponentBase(INotificationService notificationService)
        {
            this.notificationService = notificationService;
            this.notificationService.Notification += (sender, e) =>
            {
                if (this.CanHandleServerMessage(e.Notification))
                {
                    var notificationHandler = this.Notification;

                    if (notificationHandler != null)
                    {
                        notificationHandler(this, e);
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
