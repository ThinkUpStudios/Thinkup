using System;
using System.Collections.Generic;
using ThinkUp.Sdk.Plugins.PluginComponents;

namespace ThinkUp.Sdk.Plugins
{
    public interface IPlugin
    {
        event EventHandler<ServerMessageEventArgs> ServerMessage;

        bool CanHandleClientMessage(string serializedClientMessage);

        void HandleClientMessage(string serializedClientMessage);
    }

    public interface IPluginSetup : IPlugin
    {
        IEnumerable<IPluginComponentInformation> Components { get; }

        void RegisterComponent(IPluginComponent component);
    }
}
