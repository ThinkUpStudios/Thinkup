using System;
using System.Collections.Generic;
using ThinkUp.Sdk.Components;

namespace ThinkUp.Sdk.Modules
{
    public interface IModule
    {
        event EventHandler<ServerMessageEventArgs> ServerMessage;

        bool CanHandleClientMessage(string serializedClientMessage);

        void HandleClientMessage(string serializedClientMessage);
    }

    public interface IModuleSetup : IModule
    {
        IEnumerable<IComponentInformation> Components { get; }

        void RegisterComponent(IComponent component);
    }
}
