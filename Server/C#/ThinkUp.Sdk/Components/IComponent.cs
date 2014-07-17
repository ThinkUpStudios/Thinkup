using System;
using ThinkUp.Sdk.Contracts.ClientMessages;
using ThinkUp.Sdk.Contracts.ServerMessages;

namespace ThinkUp.Sdk.Components
{
    public interface IComponentInformation
    {
        string Name { get; }
    }

    public interface IComponent : IComponentInformation
    {
        event EventHandler<NotificationEventArgs> Notification;

        bool CanHandleClientMessage(ClientContract clientContract);

        bool CanHandleServerMessage(ServerContract serverContract);

        ///<exception cref="ComponentException">ComponentException</exception>
        ///<exception cref="ServiceException">ServiceException</exception>
        void HandleClientMessage(ClientContract clientContract);
    }
}
