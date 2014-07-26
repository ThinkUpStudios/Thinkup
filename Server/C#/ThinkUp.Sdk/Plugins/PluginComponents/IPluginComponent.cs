using System;
using ThinkUp.Sdk.Contracts.ClientMessages;
using ThinkUp.Sdk.Contracts.ServerMessages;

namespace ThinkUp.Sdk.Plugins.PluginComponents
{
    public interface IPluginComponentInformation
    {
        string Name { get; }
    }

    public interface IPluginComponent : IPluginComponentInformation
    {
        event EventHandler<ServerMessageEventArgs> ServerMessage;

        bool CanHandleClientMessage(ClientContract clientContract);

        bool CanHandleServerMessage(ServerContract serverContract);

        ///<exception cref="PluginComponentException">ComponentException</exception>
        ///<exception cref="ServiceException">ServiceException</exception>
        void HandleClientMessage(ClientContract clientContract);
    }
}
